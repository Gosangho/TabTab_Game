/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated September 24, 2021. Replaces all prior versions.
 *
 * Copyright (c) 2013-2021, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace Spine.Unity.Examples 
{
    public class SpineboyBeginnerView : MonoBehaviour 
    {

        #region Inspector
        [Header("Components")]
        public SpineboyBeginnerModel model;
        public SkeletonAnimation skeletonAnimation;

        public AnimationReferenceAsset run, idle, aim, shoot, jump;
        public EventDataReferenceAsset footstepEvent;

        [Header("Audio")]
        public float footstepPitchOffset = 0.2f;
        public float gunsoundPitchOffset = 0.13f;
        public AudioSource footstepSource, gunSource, jumpSource;

        [Header("Effects")]
        public ParticleSystem gunParticles;
        #endregion

        SpineBeginnerBodyState previousViewState;

        void Start () 
        {
            if (skeletonAnimation == null) 
                return;
            
            model.ShootEvent += PlayShoot;
            model.StartAimEvent += StartPlayingAim;
            model.StopAimEvent += StopPlayingAim;
            skeletonAnimation.AnimationState.Event += HandleEvent;
        }

        void HandleEvent (Spine.TrackEntry trackEntry, Spine.Event e) 
        {
            if (e.Data == footstepEvent.EventData)
                PlayFootstepSound();
            
        }

        void Update () 
        {
            // SkeletonAnimation 구성 요소가 할당되었는지 확인
            if (skeletonAnimation == null) return;

            // SpineboyBeginnerModel 구성 요소가 할당되었는지 확인
            if (model == null) return;

            // 스켈레톤의 향하는 방향을 업데이트해야 하는지 확인
            if ((skeletonAnimation.skeleton.ScaleX < 0) != model.facingLeft)
            {
                // 모델의 facialLeft 값을 기준으로 스켈레톤의 방향을 업데이트합니다.
                Turn(model.facingLeft);
            }

            // Spineboy 모델의 현재 상태 가져오기
            SpineBeginnerBodyState currentModelState = model.state;

            // 이전 프레임 이후 모델의 상태가 변경되었는지 확인
            if (previousViewState != currentModelState)
            {
                // 업데이트된 상태를 기반으로 적절한 새 안정적인 애니메이션을 재생합니다.
                PlayNewStableAnimation();
            }

            // 다음 프레임 비교를 위해 이전 상태를 현재 상태로 업데이트
            previousViewState = currentModelState;
        }


        void PlayNewStableAnimation () 
        {
            // Spineboy 모델의 현재 상태 가져오기
            SpineBeginnerBodyState newModelState = model.state;

            // 다음에 재생할 애니메이션을 저장할 변수를 선언합니다.
            Animation nextAnimation;

            // 이전 상태가 Jumping이고 새 상태가 Jumping이 아닌지 확인
            if (previousViewState == SpineBeginnerBodyState.Jumping && newModelState != SpineBeginnerBodyState.Jumping) 
            {
                // 조건이 맞으면 발소리 효과음 재생
                PlayFootstepSound();
            }

            // 재생할 다음 애니메이션을 결정하기 위해 새로운 상태를 확인하십시오.
            if (newModelState == SpineBeginnerBodyState.Jumping) 
            {
                // 새 상태가 점프 중이면 점프 애니메이션과 점프 음향 효과를 재생합니다.
                jumpSource.Play();
                nextAnimation = jump;
            } 
            else 
            {
                // 새 상태가 점프 중이 아니면 실행 중인지 유휴 상태인지 확인합니다.
                if (newModelState == SpineBeginnerBodyState.Running)
                {
                    // 새 상태가 Running이면 실행 애니메이션을 재생합니다.
                    nextAnimation = run;
                } 
                else 
                {
                    // 새 상태가 Running도 Jumping도 아닌 경우 유휴 애니메이션을 재생합니다.
                    nextAnimation = idle;
                }
            }

            // SkeletonAnimation의 트랙 0에서 nextAnimation을 계속 재생하도록 설정합니다.
            skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        }


        void PlayFootstepSound () 
        {
            footstepSource.Play();
            footstepSource.pitch = GetRandomPitch(footstepPitchOffset);
        }

        [ContextMenu("Check Tracks")]
        void CheckTracks () 
        {
            AnimationState state = skeletonAnimation.AnimationState;
            Debug.Log(state.GetCurrent(0));
            Debug.Log(state.GetCurrent(1));
        }

        #region Transient Actions
        public void PlayShoot()
        {
            // 트랙 1에서 사격 애니메이션을 재생합니다.
            TrackEntry shootTrack = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
            shootTrack.AttachmentThreshold = 1f;  // 부드러운 블렌딩을 위한 첨부 임계값 설정
            shootTrack.MixDuration = 0f;  // 다른 애니메이션과 혼합되지 않도록 혼합 기간 설정
            skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);  // 촬영 애니메이션 후 트랙을 재설정하는 빈 애니메이션 추가

            // 트랙 2에서 조준 애니메이션을 재생하여 마우스 대상을 조준합니다.
            TrackEntry aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
            aimTrack.AttachmentThreshold = 1f;  // 부드러운 블렌딩을 위한 첨부 임계값 설정
            aimTrack.MixDuration = 0f;  // 다른 애니메이션과 혼합되지 않도록 혼합 기간 설정
            skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);  // 조준 애니메이션 후 트랙을 재설정하려면 빈 애니메이션을 추가하십시오.

            gunSource.pitch = GetRandomPitch(gunsoundPitchOffset);  // 무작위 오프셋으로 총 소리의 피치를 설정합니다.
            gunSource.Play();  // 총 소리 효과 재생
            //gunParticles.randomSeed = (단위)Random.Range(0, 100); // 입자 변형에 대한 임의 시드 설정(선택 사항)
            gunParticles.Play();  // Play the gun particle effect
        }


        public void StartPlayingAim () 
        {
            // 트랙 2에서 조준 애니메이션을 재생하여 마우스 대상을 조준합니다.
            TrackEntry aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, true);
            aimTrack.AttachmentThreshold = 1f;
            aimTrack.MixDuration = 0f;
        }

        public void StopPlayingAim () 
        {
            skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
        }

        public void Turn (bool facingLeft)
        {
            skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
            // 일시적인 회전 애니메이션도 재생한 다음 ChangeStableAnimation을 호출할 수 있습니다.
        }
        #endregion

        #region Utility
        public float GetRandomPitch (float maxPitchOffset) 
        {
            return 1f + Random.Range(-maxPitchOffset, maxPitchOffset);
        }
        #endregion
    }

}