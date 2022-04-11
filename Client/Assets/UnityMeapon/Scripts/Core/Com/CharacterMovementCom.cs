#define DEBUG_CC2D_RAYS
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MeaponUnity.Core.Com
{
    /// <summary>
    /// 角色移动组件
    /// </summary>
    public class CharacterMovementCom : MonoBehaviour
    {
        #region internal types
        //射线起始位置
        struct CharacterRaycastOrigins
        {
            public Vector3 topLeft;
            public Vector3 bottomRight;
            public Vector3 bottomLeft;
        }

        public class CharacterCollisionState2D
        {
            public bool right;
            public bool left;
            public bool above;
            public bool below;
            public bool becameGroundedThisFrame;
            public bool wasGroundedLastFrame;
            public bool movingDownSlope;
            public float slopeAngle;


            public bool hasCollision()
            {
                return below || right || left || above;
            }


            public void reset()
            {
                right = left = above = below = becameGroundedThisFrame = movingDownSlope = false;
                slopeAngle = 0f;
            }


            public override string ToString()
            {
                return string.Format("[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, movingDownSlope: {4}, angle: {5}, wasGroundedLastFrame: {6}, becameGroundedThisFrame: {7}",
                                     right, left, above, below, movingDownSlope, slopeAngle, wasGroundedLastFrame, becameGroundedThisFrame);
            }
        }

        #endregion

        #region events, properties and fields

        public event Action<RaycastHit2D> onControllerCollidedEvent;
        public event Action<Collider2D> onTriggerEnterEvent;
        public event Action<Collider2D> onTriggerStayEvent;
        public event Action<Collider2D> onTriggerExitEvent;


        /// <summary>
        /// 竖向移动时是否忽略OnWayPlatform
        /// </summary>
        public bool ignoreOneWayPlatformsThisFrame;

        [SerializeField]
        [Range(0.001f, 0.3f)]
        float _skinWidth = 0.01f;

        /// <summary>
        /// defines how far in from the edges of the collider rays are cast from. If cast with a 0 extent it will often result in ray hits that are
        /// not desired (for example a foot collider casting horizontally from directly on the surface can result in a hit)
        /// </summary>
        public float skinWidth
        {
            get { return _skinWidth; }
            set
            {
                _skinWidth = value;
                recalculateDistanceBetweenRays();
            }
        }


        /// <summary>
        /// 玩家可以交互的层
        /// mask with all layers that the player should interact with
        /// </summary>
        public LayerMask platformMask = 0;

        /// <summary>
        /// 交互时可以触发事件的层
        /// mask with all layers that trigger events should fire when intersected
        /// </summary>
        public LayerMask triggerMask = 0;

        /// <summary>
        /// one-way platform层，注意只允许EdgeCollider2Ds
        /// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is because it does not support being
        /// updated anytime outside of the inspector for now.
        /// </summary>
        [SerializeField]
        LayerMask oneWayPlatformMask = 0;

        /// <summary>
        /// 上坡最大角度
        /// the max slope angle that the CC2D can climb
        /// </summary>
        /// <value>The slope limit.</value>
        [Range(0f, 90f)]
        public float slopeLimit = 30f;

        /// <summary>
        /// the threshold in the change in vertical movement between frames that constitutes jumping
        /// 构成跳跃的最小阈值, 和detalmovement.y比较
        /// </summary>
        /// <value>The jumping threshold.</value>
        public float jumpingThreshold = 0.07f;


        /// <summary>
        /// curve for multiplying speed based on slope (negative = down slope and positive = up slope)
        /// </summary>
        public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));

        [Range(2, 20)]
        public int totalHorizontalRays = 8;
        [Range(2, 20)]
        public int totalVerticalRays = 4;


        /// <summary>
        /// this is used to calculate the downward ray that is cast to check for slopes. We use the somewhat arbitrary value 75 degrees
        /// to calculate the length of the ray that checks for slopes.
        /// </summary>
        float _slopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);


        //[HideInInspector]
        //[NonSerialized]
        //public new Transform transform;
        [HideInInspector]
        [NonSerialized]
        public BoxCollider2D boxCollider;
        [HideInInspector]
        [NonSerialized]
        public Rigidbody2D rigidBody2D;

        [HideInInspector]
        [NonSerialized]
        public CharacterCollisionState2D collisionState = new CharacterCollisionState2D();
        [HideInInspector]
        [NonSerialized]
        public Vector3 velocity;
        public bool isGrounded { get { return collisionState.below; } }

        const float kSkinWidthFloatFudgeFactor = 0.001f;

        #endregion


        /// <summary>
        /// holder for our raycast origin corners (TR, TL, BR, BL)
        /// </summary>
        CharacterRaycastOrigins _raycastOrigins;

        /// <summary>
        /// stores our raycast hit during movement
        /// </summary>
        RaycastHit2D _raycastHit;

        /// <summary>
        /// stores any raycast hits that occur this frame. we have to store them in case we get a hit moving
        /// horizontally and vertically so that we can send the events after all collision state is set
        /// </summary>
        List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D>(2);

        // horizontal/vertical movement data
        float _verticalDistanceBetweenRays;  //水平移动时，从下向上的射线间隔
        float _horizontalDistanceBetweenRays;

        // we use this flag to mark the case where we are travelling up a slope and we modified our delta.y to allow the climb to occur.
        // the reason is so that if we reach the end of the slope we can make an adjustment to stay grounded
        bool _isGoingUpSlope = false;


        #region Monobehaviour
        public void Init(CharacterMovementData data)
        {
            // add our one-way platforms to our normal platform mask so that we can land on them from above
            platformMask = data.PlatformMask;
            platformMask |= data.OneWayPlatformMask;
            triggerMask = data.InteractMask;
            // cache some components
            //transform = GetComponent<Transform>();
            boxCollider = GetComponent<BoxCollider2D>();
            rigidBody2D = GetComponent<Rigidbody2D>();

            onTriggerEnterEvent += data.OnTriggerEnterEvent;
            onTriggerStayEvent += data.OnTriggerStayEvent;
            onTriggerExitEvent += data.OnTriggerExitEvent;

            // here, we trigger our properties that have setters with bodies
            skinWidth = _skinWidth;

            // we want to set our CC2D to ignore all collision layers except what is in our triggerMask
            for (var i = 0; i < 32; i++)
            {
                // see if our triggerMask contains this layer and if not ignore it
                if ((triggerMask.value & 1 << i) == 0)
                    Physics2D.IgnoreLayerCollision(gameObject.layer, i);
            }
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (onTriggerEnterEvent != null)
                onTriggerEnterEvent(col);
        }


        public void OnTriggerStay2D(Collider2D col)
        {
            if (onTriggerStayEvent != null)
                onTriggerStayEvent(col);
        }


        public void OnTriggerExit2D(Collider2D col)
        {
            if (onTriggerExitEvent != null)
                onTriggerExitEvent(col);
        }

        #endregion


        [System.Diagnostics.Conditional("DEBUG_CC2D_RAYS")]
        void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            Debug.DrawRay(start, dir, color);
        }


        #region Public

        /// <summary>
        /// attempts to move the character to position + deltaMovement. Any colliders in the way will cause the movement to
        /// stop when run into.
        /// </summary>
        /// <param name="deltaMovement">Delta movement.</param>
        public void move(Vector3 deltaMovement)
        {
            Debug.DrawLine(transform.position, transform.position + deltaMovement * 10, Color.cyan);

            // save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
            collisionState.wasGroundedLastFrame = collisionState.below;

            // clear our state
            collisionState.reset();
            _raycastHitsThisFrame.Clear();
            _isGoingUpSlope = false;

            //计算Box
            primeRaycastOrigins();

            // first, we check for a slope below us before moving
            // only check slopes if we are going down and grounded
            // 在移动前首先检查滑动
            if (deltaMovement.y < 0f && collisionState.wasGroundedLastFrame)
            {
                handleVerticalSlope(ref deltaMovement);
            }

            // now we check movement in the horizontal dir
            // 水平方向的移动
            if (deltaMovement.x != 0f)
            {
                moveHorizontally(ref deltaMovement);
            }

            // next, check movement in the vertical dir
            // 垂直方向的移动
            if (deltaMovement.y != 0f)
                moveVertically(ref deltaMovement);

            // move then update our state
            // 移动完成后更新状态
            deltaMovement.z = 0;
            transform.Translate(deltaMovement, Space.World);

            // only calculate velocity if we have a non-zero deltaTime
            if (Time.deltaTime > 0f)
                velocity = deltaMovement / Time.deltaTime;

            // set our becameGrounded state based on the previous and current collision state
            if (!collisionState.wasGroundedLastFrame && collisionState.below)
                collisionState.becameGroundedThisFrame = true;

            // if we are going up a slope we artificially set a y velocity so we need to zero it out here
            if (_isGoingUpSlope)
                velocity.y = 0;

            // send off the collision events if we have a listener
            if (onControllerCollidedEvent != null)
            {
                for (var i = 0; i < _raycastHitsThisFrame.Count; i++)
                    onControllerCollidedEvent(_raycastHitsThisFrame[i]);
            }

            ignoreOneWayPlatformsThisFrame = false;
        }


        /// <summary>
        /// moves directly down until grounded
        /// </summary>
        public void warpToGrounded()
        {
            do
            {
                move(new Vector3(0, -1f, 0));
            } while (!isGrounded);
        }


        /// <summary>
        /// this should be called anytime you have to modify the BoxCollider2D at runtime. It will recalculate the distance between the rays used for collision detection.
        /// It is also used in the skinWidth setter in case it is changed at runtime.
        /// </summary>
        public void recalculateDistanceBetweenRays()
        {
            // figure out the distance between our rays in both directions
            // horizontal
            var colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2f * _skinWidth);
            _verticalDistanceBetweenRays = colliderUseableHeight / (totalHorizontalRays - 1);

            // vertical
            var colliderUseableWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2f * _skinWidth);
            _horizontalDistanceBetweenRays = colliderUseableWidth / (totalVerticalRays - 1);
        }

        #endregion


        #region Movement Methods

        /// <summary>
        /// resets the raycastOrigins to the current extents of the box collider inset by the skinWidth. It is inset
        /// to avoid casting a ray from a position directly touching another collider which results in wonky normal data.
        /// </summary>
        /// <param name="futurePosition">Future position.</param>
        /// <param name="deltaMovement">Delta movement.</param>
        void primeRaycastOrigins()
        {
            // our raycasts need to be fired from the bounds inset by the skinWidth
            var modifiedBounds = boxCollider.bounds;
            modifiedBounds.Expand(-2f * _skinWidth);

            _raycastOrigins.topLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y);
            _raycastOrigins.bottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);
            _raycastOrigins.bottomLeft = modifiedBounds.min;
        }


        /// <summary>
        /// we have to use a bit of trickery in this one. The rays must be cast from a small distance inside of our
        /// collider (skinWidth) to avoid zero distance rays which will get the wrong normal. Because of this small offset
        /// we have to increase the ray distance skinWidth then remember to remove skinWidth from deltaMovement before
        /// actually moving the player
        /// 这个地方需要使用一些技巧，射线在碰撞盒内部时必须以很小的距离发出射线，以避免0射线得到错误的法线
        /// </summary>
        void moveHorizontally(ref Vector3 deltaMovement)
        {
            bool isGoingRight = deltaMovement.x > 0;  //x>0时在右边
            float rayDistance = Mathf.Abs(deltaMovement.x) + _skinWidth;
            Vector2 rayDirection = isGoingRight ? Vector2.right : Vector2.left;
            Vector3 initialRayOrigin = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;

            for (int i = 0; i < totalHorizontalRays; i++)
            {
                Vector2 ray = new Vector2(initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays);

                DrawRay(ray, rayDirection * rayDistance, Color.red);

                // if we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
                // walk up sloped oneWayPlatforms
                // 如果我们在地面，这就允许我们走上倾斜的OnWayPlatform（因为只检测脚底最下面的射线）
                if (i == 0 && collisionState.wasGroundedLastFrame)
                    _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask);
                else
                    _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, platformMask & ~oneWayPlatformMask);

                if (_raycastHit)
                {
                    // the bottom ray can hit a slope but no other ray can so we have special handling for these cases
                    if (i == 0 && handleHorizontalSlope(ref deltaMovement, Vector2.Angle(_raycastHit.normal, Vector2.up)))
                    {
                        _raycastHitsThisFrame.Add(_raycastHit);
                        // if we weren't grounded last frame, that means we're landing on a slope horizontally.
                        // this ensures that we stay flush to that slope
                        if (!collisionState.wasGroundedLastFrame)
                        {
                            float flushDistance = Mathf.Sign(deltaMovement.x) * (_raycastHit.distance - skinWidth);
                            transform.Translate(new Vector2(flushDistance, 0));
                        }
                        break;
                    }

                    // set our new deltaMovement and recalculate the rayDistance taking it into account
                    deltaMovement.x = _raycastHit.point.x - ray.x;
                    rayDistance = Mathf.Abs(deltaMovement.x);

                    // remember to remove the skinWidth from our deltaMovement
                    if (isGoingRight)
                    {
                        deltaMovement.x -= _skinWidth;
                        collisionState.right = true;
                    }
                    else
                    {
                        deltaMovement.x += _skinWidth;
                        collisionState.left = true;
                    }

                    _raycastHitsThisFrame.Add(_raycastHit);

                    // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                    // than the width + fudge bail out because we have a direct impact
                    if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
                        break;
                }
            }
        }


        /// <summary>
        /// handles adjusting deltaMovement if we are going up a slope.
        /// </summary>
        /// <returns><c>true</c>, if horizontal slope was handled, <c>false</c> otherwise.</returns>
        /// <param name="deltaMovement">Delta movement.</param>
        /// <param name="angle">Angle.</param>
        bool handleHorizontalSlope(ref Vector3 deltaMovement, float angle)
        {
            // disregard 90 degree angles (walls)
            // 90度就是墙
            if (Mathf.RoundToInt(angle) == 90)
                return false;

            // if we can walk on slopes and our angle is small enough we need to move up
            // 角度越小说明坡度越小
            if (angle < slopeLimit)
            {
                // we only need to adjust the deltaMovement if we are not jumping
                // TODO: this uses a magic number which isn't ideal! The alternative is to have the user pass in if there is a jump this frame
                if (deltaMovement.y < jumpingThreshold)
                {
                    // apply the slopeModifier to slow our movement up the slope
                    float slopeModifier = slopeSpeedMultiplier.Evaluate(angle);
                    deltaMovement.x *= slopeModifier;

                    // we dont set collisions on the sides for this since a slope is not technically a side collision.
                    // smooth y movement when we climb. we make the y movement equivalent to the actual y location that corresponds
                    // to our new x location using our good friend Pythagoras
                    deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
                    var isGoingRight = deltaMovement.x > 0;

                    // safety check. we fire a ray in the direction of movement just in case the diagonal we calculated above ends up
                    // going through a wall. if the ray hits, we back off the horizontal movement to stay in bounds.
                    Vector3 ray = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
                    RaycastHit2D raycastHit;
                    if (collisionState.wasGroundedLastFrame)
                        raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude, platformMask);
                    else
                        raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude, platformMask & ~oneWayPlatformMask);

                    if (raycastHit)
                    {
                        // we crossed an edge when using Pythagoras calculation, so we set the actual delta movement to the ray hit location
                        deltaMovement = (Vector3)raycastHit.point - ray;
                        if (isGoingRight)
                            deltaMovement.x -= _skinWidth;
                        else
                            deltaMovement.x += _skinWidth;
                    }

                    _isGoingUpSlope = true;
                    collisionState.below = true;
                    collisionState.slopeAngle = -angle;
                }
            }
            else // too steep. get out of here
            {
                // 坡度太大，水平方向置0
                deltaMovement.x = 0;
            }

            return true;
        }


        void moveVertically(ref Vector3 deltaMovement)
        {
            var isGoingUp = deltaMovement.y > 0;
            var rayDistance = Mathf.Abs(deltaMovement.y) + _skinWidth;
            var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
            var initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

            // apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
            initialRayOrigin.x += deltaMovement.x;

            // if we are moving up, we should ignore the layers in oneWayPlatformMask
            var mask = platformMask;
            if ((isGoingUp && !collisionState.wasGroundedLastFrame) || ignoreOneWayPlatformsThisFrame)
                mask &= ~oneWayPlatformMask;

            for (var i = 0; i < totalVerticalRays; i++)
            {
                var ray = new Vector2(initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y);

                DrawRay(ray, rayDirection * rayDistance, Color.red);
                _raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, mask);
                if (_raycastHit)
                {
                    // set our new deltaMovement and recalculate the rayDistance taking it into account
                    deltaMovement.y = _raycastHit.point.y - ray.y;
                    rayDistance = Mathf.Abs(deltaMovement.y);

                    // remember to remove the skinWidth from our deltaMovement
                    if (isGoingUp)
                    {
                        deltaMovement.y -= _skinWidth;
                        collisionState.above = true;
                    }
                    else
                    {
                        deltaMovement.y += _skinWidth;
                        collisionState.below = true;
                    }

                    _raycastHitsThisFrame.Add(_raycastHit);

                    // this is a hack to deal with the top of slopes. if we walk up a slope and reach the apex we can get in a situation
                    // where our ray gets a hit that is less then skinWidth causing us to be ungrounded the next frame due to residual velocity.
                    if (!isGoingUp && deltaMovement.y > 0.00001f)
                        _isGoingUpSlope = true;

                    // we add a small fudge factor for the float operations here. if our rayDistance is smaller
                    // than the width + fudge bail out because we have a direct impact
                    if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
                        break;
                }
            }
        }


        /// <summary>
        /// checks the center point under the BoxCollider2D for a slope. If it finds one then the deltaMovement is adjusted so that
        /// the player stays grounded and the slopeSpeedModifier is taken into account to speed up movement.
        /// 检查垂直方向的滑动，也就是坡上
        /// </summary>
        /// <param name="deltaMovement">Delta movement.</param>
        private void handleVerticalSlope(ref Vector3 deltaMovement)
        {
            // slope check from the center of our collider
            float centerXOfCollider = (_raycastOrigins.bottomLeft.x + _raycastOrigins.bottomRight.x) * 0.5f;
            Vector2 rayDirection = Vector2.down;  //射线方向朝下

            // the ray distance is based on our slopeLimit
            float slopeCheckRayDistance = _slopeLimitTangent * (_raycastOrigins.bottomRight.x - centerXOfCollider);

            //脚底中心点
            Vector2 slopeRay = new Vector2(centerXOfCollider, _raycastOrigins.bottomLeft.y);
            DrawRay(slopeRay, rayDirection * slopeCheckRayDistance, Color.magenta);

            //仅在platformMask滑动
            _raycastHit = Physics2D.Raycast(slopeRay, rayDirection, slopeCheckRayDistance, platformMask);
            if (_raycastHit)
            {
                // bail out if we have no slope
                // 如果滑动角度为0，则返回，平地时角度一定为0
                Debug.DrawRay(slopeRay, _raycastHit.normal * slopeCheckRayDistance, Color.blue);
                float angle = Vector2.Angle(_raycastHit.normal, Vector2.up);
                if (angle == 0)
                    return;

                // we are moving down the slope if our normal and movement direction are in the same x direction
                // 法向量方向和移动方向同向
                bool isMovingDownSlope = Mathf.Sign(_raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
                if (isMovingDownSlope)
                {
                    // going down we want to speed up in most cases so the slopeSpeedMultiplier curve should be > 1 for negative angles
                    // 从AnimationCurve中取值 (定义了-90度到90度)
                    float slopeModifier = slopeSpeedMultiplier.Evaluate(-angle);
                    // we add the extra downward movement here to ensure we "stick" to the surface below
                    // 增加一些移动确保粘在地面
                    // y:射线碰撞点 - 脚底中心点 - "衣服长度"
                    // x:滑动值(此帧移动的x距离 * 曲线中取到的值) 沿z轴旋转坡度的角度
                    // 注意deltaMovement为ref
                    deltaMovement.y += _raycastHit.point.y - slopeRay.y - skinWidth;
                    deltaMovement = new Vector3(0, deltaMovement.y, 0) +
                                    (Quaternion.AngleAxis(-angle, Vector3.forward) * new Vector3(deltaMovement.x * slopeModifier, 0, 0));

                    // 设置向下滑动的状态 和 角度
                    collisionState.movingDownSlope = true;
                    collisionState.slopeAngle = angle;
                }
            }
        }

        #endregion

    }
}
