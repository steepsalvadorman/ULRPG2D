using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MovingState : FSMState<EnemyController>
    {
        private Vector3 mDirection;

        public MovingState(EnemyController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<EnemyController>(
                isValid : () => {
                    return Vector3.Distance(
                        mController.transform.position,
                        mController.Player.transform.position
                    ) >= mController.WakeDistance;
                },
                getNextState : () => {
                    return new IdleState(mController);
                }
            ));

            Transitions.Add(new FSMTransition<EnemyController>(
                isValid : () => {
                    return Vector3.Distance(
                        mController.transform.position,
                        mController.Player.transform.position
                    ) <= mController.AttackDistance;
                },
                getNextState : () => {
                    return new AttackingState(mController);
                }
            ));
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter MovingState");
            mController.animator.SetBool("IsMoving", true);
        }

        public override void OnExit()
        {
            Debug.Log("OnExit MovingState");
        }

        public override void OnUpdate(float deltaTime)
        {
            var playerPosition = mController.Player.transform.position;
            var enemyPosition = mController.transform.position;

            mDirection = (playerPosition - enemyPosition).normalized;

            if (mDirection != Vector3.zero)
            {
                mController.animator.SetFloat("Horizontal", mDirection.x);
                mController.animator.SetFloat("Vertical", mDirection.y);
            }

            mController.rb.MovePosition(
                mController.transform.position + (mDirection * mController.Speed * deltaTime)
            );
        }
    }
}
