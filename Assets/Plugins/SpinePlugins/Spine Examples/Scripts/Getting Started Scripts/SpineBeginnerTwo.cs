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

namespace Spine.Unity.Examples {
	public class SpineBeginnerTwo : MonoBehaviour
	{

		#region Inspector
		// [SpineAnimation] 속성은 SkeletonAnimation에서 오는 Spine 애니메이션 이름의 Inspector 드롭다운을 허용합니다.
		[SpineAnimation]
		public string runAnimationName;

		[SpineAnimation]
		public string idleAnimationName;

		[SpineAnimation]
		public string walkAnimationName;

		[SpineAnimation]
		public string shootAnimationName;

		[Header("Transitions")]
		[SpineAnimation]
		public string idleTurnAnimationName;

		[SpineAnimation]
		public string runToIdleAnimationName;

		public float runWalkDuration = 1.5f;
		#endregion

		SkeletonAnimation skeletonAnimation;

		// Spine.AnimationState 및 Spine.Skeleton은 Unity 직렬화 개체가 아닙니다. 인스펙터에서 필드로 볼 수 없습니다.
		public Spine.AnimationState spineAnimationState;
		public Spine.Skeleton skeleton;

		void Start () 
		{
			// Start 또는 Later에서 이러한 AnimationState 및 Skeleton 참조를 가져오는지 확인하십시오.
			// Awake에서 가져오고 사용하는 것은 기본 실행 순서로 보장되지 않습니다.
			skeletonAnimation = GetComponent<SkeletonAnimation>();
			spineAnimationState = skeletonAnimation.AnimationState;
			skeleton = skeletonAnimation.Skeleton;

			StartCoroutine(DoDemoRoutine());
		}

		/// 이것은 무한히 반복되는 Unity 코루틴입니다. 자세한 내용은 코루틴에 대한 Unity 문서를 참조하세요.
		IEnumerator DoDemoRoutine () 
		{
			while (true) 
			{
				// SetAnimation은 애니메이션을 설정하는 기본적인 방법입니다.
				// SetAnimation은 애니메이션을 설정하고 처음부터 재생을 시작합니다.
				// 일반적인 실수: Update에서 계속 호출하면 애니메이션의 첫 번째 포즈가 계속 표시됩니다. 그렇게 하지 마십시오.

				spineAnimationState.SetAnimation(0, walkAnimationName, true);
				yield return new WaitForSeconds(runWalkDuration);

				spineAnimationState.SetAnimation(0, runAnimationName, true);
				yield return new WaitForSeconds(runWalkDuration);

				// AddAnimation은 이전 애니메이션이 끝난 후 재생할 애니메이션을 대기열에 넣습니다.
				spineAnimationState.SetAnimation(0, runToIdleAnimationName, false);
				spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
				yield return new WaitForSeconds(5f);

				skeleton.ScaleX = -1;      // 스켈레톤을 사용하면 스켈레톤을 뒤집을 수 있습니다.
				spineAnimationState.SetAnimation(0, idleTurnAnimationName, false);
				spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
				yield return new WaitForSeconds(0.5f);
				
				skeleton.ScaleX = 1;
				spineAnimationState.SetAnimation(0, idleTurnAnimationName, false);
				spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
				yield return new WaitForSeconds(0.5f);

			}
		}

	}

}
