using System.Collections;
using UnityEngine;

namespace Rpg.Utils
{

    /// <summary>
    /// 
    /// </summary>
    public class Effects : MonoBehaviour
    {

        [Header("Application on")]

        [SerializeField]
        private bool applicationOnObject;
        [SerializeField]
        private bool applicationOnUi;




        [Header("Efects")]

        [SerializeField]
        private bool scaleEffect;
        [SerializeField]
        private bool rotateEffect;
        [SerializeField]
        private bool moveEffect;




        [Header("Scale Effect")]

        [SerializeField]
        private bool notUseOffsetOnFisrtUse;
        [SerializeField]
        private bool notScaleFisrtUse;
        [SerializeField]
        private int framesScaleAnimation;
        [SerializeField]
        private float framesUnscaleAnimation;
        [SerializeField]
        private float scaleTimeOffset;
        [SerializeField]
        private float minScale;
        [SerializeField]
        private float maxScale;

        private float scaleCounter;
        private float nextScale;




        [Header("Rotation Effect")]

        [SerializeField]
        private bool PermanentRotation;
        [SerializeField]
        private bool randomizeRotationOnStart;
        /*[SerializeField]
          private bool randomizeRotationEachRandommizeTime;
          [SerializeField]
          private float randomizeFrameRate;*/
        [SerializeField]
        private Vector3 RotateSpeed;




        [Header("Move Effect")]

        [SerializeField]
        private bool XAxis;
        [SerializeField]
        private bool YAxis;
        [SerializeField]
        private bool ZAxis;
        [SerializeField]
        private int frameToGoOnGoal;
        [SerializeField]
        private int frameToReturnToStartPosition;
        [SerializeField]
        private int moveOffsetTime;
        [SerializeField]
        private Vector3 minPosition;
        [SerializeField]
        private Vector3 goalPosition;
      
        private float moveCounter;
        private float nextX;
        private float nextY;
        private float nextZ;

        protected void Start()
        {
            if (scaleEffect)
            {
                if (applicationOnObject)
                {
                    scaleCounter = -scaleTimeOffset;
                    StartCoroutine(Scale(false));
                }
                else
                {
                    if (notScaleFisrtUse)
                    {
                        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                        notScaleFisrtUse = false;

                    }
                    else
                    {
                        transform.localScale = new Vector3(minScale, minScale, minScale);
                    }
                }
            }
            if (rotateEffect)
            {
                if (randomizeRotationOnStart) RandomizeRotationSpeed();
            }
            if (moveEffect)
            {
                if (applicationOnObject)
                {

                    goalPosition = new Vector3(transform.position.x + goalPosition.x, transform.position.y + goalPosition.y, transform.position.z + goalPosition.z);
                    minPosition = new Vector3(transform.position.x + minPosition.x, transform.position.y + minPosition.y, transform.position.z + minPosition.z);
                    moveCounter = -moveOffsetTime;
                    StartCoroutine(Move(true));

                }
            }

        }

        protected void OnEnable()
        {
            if (scaleEffect)
            {
                if (!applicationOnObject)
                {
                    scaleCounter = -scaleTimeOffset;
                    if (notUseOffsetOnFisrtUse)
                    {
                        scaleCounter = 0;
                        notUseOffsetOnFisrtUse = false;
                    }
                    if (notScaleFisrtUse) return;
                    transform.localScale = new Vector3(minScale, minScale, minScale);
                    StartCoroutine(Scale(true));
                }
            }


        }


        protected void Update()
        {
            if (rotateEffect && PermanentRotation) transform.Rotate(RotateSpeed * Time.deltaTime);
        }

        public void DisableObject()
        {
            scaleCounter = 0;
            transform.localScale = new Vector3(minScale, minScale, minScale);
            if (gameObject.active) StartCoroutine(Scale(false));
        }

        private void RandomizeRotationSpeed()
        {
            RotateSpeed = new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));
        }

        private IEnumerator Scale(bool toScale)
        {

            if (framesScaleAnimation == 0)
            {
                transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                yield break;
            }
            if (scaleCounter > 0)
            {
                if ((scaleCounter == framesScaleAnimation && toScale) || (scaleCounter == framesUnscaleAnimation && !toScale))
                {
                    if (toScale)
                    {
                        if (applicationOnObject)
                        {
                            scaleCounter = 0;
                            transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                            StartCoroutine(Scale(false));
                        }
                        else
                        {
                            transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                        }

                    }
                    else
                    {
                        if (applicationOnObject)
                        {
                            scaleCounter = 0;
                            transform.localScale = new Vector3(minScale, minScale, minScale);
                            StartCoroutine(Scale(true));
                        }
                        else
                        {
                            if (gameObject.name.IndexOf("Button") != -1)
                            {
                                transform.localScale = new Vector3(minScale, minScale, minScale);
                                yield break;
                            }
                            gameObject.SetActive(false);
                        }
                    }
                    yield break;
                }
                if (toScale) { nextScale = minScale + (maxScale - minScale) * (scaleCounter / framesScaleAnimation); }
                else { nextScale = (maxScale) - (maxScale - minScale) * (scaleCounter / framesUnscaleAnimation); }
                if (nextScale > maxScale) nextScale = maxScale;
                if (nextScale < minScale) nextScale = minScale;
                transform.localScale = new Vector3(nextScale, nextScale, nextScale);
            }

            yield return new WaitForFixedUpdate();
            scaleCounter++;
            StartCoroutine(Scale(toScale));

        }

        private IEnumerator Move(bool toMove)
        {
            if (frameToGoOnGoal == 0)
            {
                transform.localPosition = minPosition;
                yield break;
            }
            if (moveCounter > 0)
            {
                if ((moveCounter == frameToGoOnGoal && toMove) || (moveCounter == frameToReturnToStartPosition && !toMove))
                {
                    if (toMove)
                    {
                        if (applicationOnObject)
                        {
                            moveCounter = 0;
                            transform.localPosition = goalPosition;
                            StartCoroutine(Move(false));
                        }
                        else
                        {
                            transform.localPosition = goalPosition;
                        }

                    }
                    else
                    {
                        if (applicationOnObject)
                        {
                            moveCounter = 0;
                            transform.localPosition = minPosition;
                            StartCoroutine(Move(true));
                        }
                    }
                    yield break;
                }
            }
            if (toMove)
            {
                nextX = minPosition.x + (goalPosition.x - minPosition.x) * (moveCounter / frameToGoOnGoal);
                nextY = minPosition.y + (goalPosition.y - minPosition.y) * (moveCounter / frameToGoOnGoal);
                nextZ = minPosition.z + (goalPosition.z - minPosition.z) * (moveCounter / frameToGoOnGoal);
                if (!YAxis) nextY = transform.position.y;
                if (!XAxis) nextX = transform.position.x;
                if (!ZAxis) nextZ = transform.position.z;
            }
            else
            {
                nextX = (goalPosition.x) - (goalPosition.x - minPosition.x) * (moveCounter / frameToReturnToStartPosition);
                nextY = (goalPosition.y) - (goalPosition.y - minPosition.y) * (moveCounter / frameToReturnToStartPosition);
                nextZ = (goalPosition.z) - (goalPosition.z - minPosition.z) * (moveCounter / frameToReturnToStartPosition);
                if (!YAxis) nextY = transform.position.y;
                if (!XAxis) nextX = transform.position.x;
                if (!ZAxis) nextZ = transform.position.z;
            }
            if (XAxis)
            {
                if (nextX > goalPosition.x) nextX = goalPosition.x;
                if (nextX < minPosition.x) nextX = minPosition.x;
            }
            if (YAxis)
            {
                if (nextY > goalPosition.y) nextY = goalPosition.y;
                if (nextY < minPosition.y) nextY = minPosition.y;
            }
            if (ZAxis)
            {
                if (nextZ > goalPosition.z) nextZ = goalPosition.z;
                if (nextZ < minPosition.z) nextZ = minPosition.z;
            }
         

            transform.localPosition = new Vector3(nextX, nextY, nextZ);
            yield return new WaitForFixedUpdate();
            moveCounter++;
            StartCoroutine(Move(toMove));

        }
    }

}
