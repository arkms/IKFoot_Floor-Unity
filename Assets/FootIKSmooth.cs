using UnityEngine;

public class FootIKSmooth : MonoBehaviour
{
    public bool IkActive= true;
    [Range(0f, 1f)]
    public float WeightPositionRight= 1f;
	[Range(0f, 1f)]
	public float WeightRotationRight= 0f;
    [Range(0f, 1f)]
    public float WeightPositionLeft = 1f;
	[Range(0f, 1f)]
	public float WeightRotationLeft = 0f;

    Animator anim;
    [Tooltip("Offset for Foot position")]
    public Vector3 offsetFoot;
    [Tooltip("Layer where foot can adjust to surface")]
    public LayerMask RayMask;


    [Header("DEBUG")]
    //This line can be delete
    public bool DebugEnable = true;
    public Transform FootRight = null;
    public Transform FootLeft = null;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    RaycastHit hit;

    void OnAnimatorIK(int _layerIndex)
    {
        if(IkActive)
        {
			Vector3 FootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot); //get current foot position (After animation apply)
            if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask)) //Throw raycast to down
            {
				anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, WeightPositionRight);
				anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, WeightRotationRight);
				anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + offsetFoot); //Set foot where raycast hit

                // CAN BE DELETE ------------------------------------------------
                if(DebugEnable)
                {
                    Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, FootLeft.right), Color.blue);
                    Debug.DrawLine(FootLeft.position, FootLeft.position + FootLeft.right, Color.yellow);
                }
                // FINISH CAN BE DELETE -----------------------------------------------

                if (WeightRotationRight > 0f) //adjust foot if is enable
                {
                    //Little formula to calculate foot rotation (This can be better)
                    Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, footRotation);
                }
            }
            else //Raycast does not hit anything, so we keep original position and rotation
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            }

			FootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot); //get current foot position
            if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask)) //Throw raycast to down
            {
				anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, WeightPositionLeft);
				anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, WeightRotationLeft);
				anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + offsetFoot);

                // CAN BE DELETE ------------------------------------------------
                if (DebugEnable)
                {
                    Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, FootLeft.right), Color.blue);
                    Debug.DrawLine(FootLeft.position, FootLeft.position + FootLeft.right, Color.yellow);
                }
                // FINISH CAN BE DELETE -----------------------------------------------

                if (WeightRotationLeft > 0f) //adjust foot if is enable
                {
                    //Little formula to calculate foot rotation (This can be better)
                    Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, footRotation);
                }
            }
            else //Raycast does not hit anything, so we keep original position and rotation
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
            }
        }
        else //IK is turn off, we not set anything
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
        }

    } //End OnAnimatorIK()
}
