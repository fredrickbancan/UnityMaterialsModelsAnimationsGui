using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// simple script for rendering spring joints
/// </summary>
public class SpringRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lrPrefab = null;

    private List<LineRenderer> lrList = null;
    void Start()
    {
        lrList = new List<LineRenderer>();
    }

    /// <summary>
    /// For each spring join attached to the object, have a line renderer for it and render a line between the two objects.
    /// This code keeps the list of line renderers the same size as the number of spring joints, removing un-used ones and adding required ones.
    /// This makes it so this script and render as many spring joints as needed.
    /// </summary>
    void Update()
    {
        SpringJoint[] joints = gameObject.GetComponents<SpringJoint>();

        if (joints != null && joints.Length > 0)
        {
            int dif = lrList.Count - joints.Length;

            for (int i = 0; i < dif; i++)
            {
                Destroy(lrList[(joints.Length - 1) + i]);
                lrList.RemoveAt((joints.Length - 1) + i);
            }

            dif = joints.Length - lrList.Count;

            for(int i = 0; i < dif; i++)
            {
                lrList.Add(Instantiate<LineRenderer>(lrPrefab, transform));
                lrList[lrList.Count-1].enabled = true;
            }

            for (int i = 0; i < joints.Length; i++)
            {
                lrList[i].SetPosition(0, joints[i].connectedBody.position);
                lrList[i].SetPosition(1, transform.position);
            }
        }
        else
        {
            foreach (LineRenderer l in lrList)
            {
                Destroy(l);
            }
            lrList.Clear();
        }
    }
}
