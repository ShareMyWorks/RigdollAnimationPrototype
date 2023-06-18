using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigtableJointCtrl : MonoBehaviour
{
    private Quaternion jointQ, invJointQ, initQ;
    private ConfigurableJoint joint;
    public Transform animRoot;
    private Transform targetTrans;
    public float jointDriveSpring = 10000;
    public float jointDriveDamper = 50;
    public float jointDriveMaxforce = float.MaxValue;
    void Awake()
    {
        JointDrive jDrive = new JointDrive();
        jDrive.positionDamper = jointDriveDamper;
        jDrive.positionSpring = jointDriveSpring;
        jDrive.maximumForce = jointDriveMaxforce;

        joint = GetComponent<ConfigurableJoint>();
        if(joint == null)
        {
            joint = this.gameObject.AddComponent<ConfigurableJoint>();
        }

        joint.connectedBody = joint.transform.parent.GetComponent<Rigidbody>();
        joint.angularXDrive = jDrive;
        joint.angularYZDrive = jDrive;

        var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        var up = Vector3.Cross(forward, joint.axis).normalized;
        jointQ = Quaternion.LookRotation(forward, up);
        invJointQ = Quaternion.Inverse(jointQ);
        initQ = joint.transform.localRotation;

        var transfs=animRoot.GetComponentsInChildren<Transform>();
        foreach(Transform t in transfs)
        {
            if (t.name == this.transform.name)
            {
                targetTrans = t;
            }
        }
    }

    private void FixedUpdate()
    {
        joint.targetRotation = invJointQ * Quaternion.Inverse(targetTrans.localRotation) * initQ * jointQ;
    }
}
