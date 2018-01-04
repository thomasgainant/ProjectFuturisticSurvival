using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static float signedAngle(Vector3 a, Vector3 b)
    {
        float angle = Vector3.Angle(a, b); // calculate angle
                                           // assume the sign of the cross product's Y component:
        return angle * Mathf.Sign(Vector3.Cross(a, b).y);
    }

    public static RaycastHit[] SortCollisions(RaycastHit[] hits)
    {
        List<RaycastHit> list = new List<RaycastHit>();

        if (hits.Length > 0)
        {
            list.Add(hits[0]);

            for (int i = 1; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                bool added = false;

                for (int j = 0; j < list.Count; j++)
                {
                    if (hit.distance > list[j].distance)
                    {
                        continue;
                    }
                    else if (hit.distance <= list[j].distance)
                    {
                        List<RaycastHit> newList = new List<RaycastHit>();

                        for (int k = 0; k < j; k++)
                        {
                            newList.Add(list[k]);
                        }
                        newList.Add(hit);
                        for (int k = j; k < list.Count; k++)
                        {
                            newList.Add(list[k]);
                        }

                        list = newList;
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    list.Add(hit);
                }
            }
        }

        RaycastHit[] result = new RaycastHit[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            result[i] = list[i];
        }
        return result;
    }

}
