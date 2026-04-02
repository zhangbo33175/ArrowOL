using Honor.Runtime;
using UnityEngine;

namespace Honor.Runtime
{
    public class CameraOutlineAnimation : MonoBehaviour
    {
        bool pingPong = false;

        void Start()
        {

        }

        void Update()
        {
            Color c = GetComponent<CameraOutlineBuffer>().LineColor0;

            if (pingPong)
            {
                c.a += Time.deltaTime;

                if (c.a >= 1)
                {
                    pingPong = false;
                }
            }
            else
            {
                c.a -= Time.deltaTime;

                if (c.a <= 0)
                {
                    pingPong = true;
                }
            }

            c.a = Mathf.Clamp01(c.a);
            GetComponent<CameraOutlineBuffer>().LineColor0 = c;
            GetComponent<CameraOutlineBuffer>().UpdateMaterialsPublicProperties();
        }
    }

}
