using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public Transform muzzle;
    private PlayerMove playerMove;

    public Animator animator;

    [Header("Skill")]
    public SkillBase nowSkill;
    public int poolSize = 15;
    public float range = 6;

    private bool isClicked = false;
    private float percent = 0, maxPercent = 100;

    [Header("Charge")]
    public int index = 0;
    public int[] powerGrade;
    public int chargeSpeed = 20;

    public Image chargeBar;

    private UnitInfo unitInfo;

    public bool isDead = false;

    public void SkillButtonDown()
    {
        if (isDead) return;
        isClicked = true;
    }

    public void SkillButtonUp()
    {
        if (isDead) return;
        if (index > 0)
        {
            animator.SetBool("IsAttacking", true);
            UseSkill();
            Invoke("FinishSkill", 0.1f);
        }

        isClicked = false;
        percent = index = 0;
        chargeBar.fillAmount = 0;
    }

    private void UseSkill()
    {
        nowSkill.UseSkill(index, range, muzzle.position, playerMove.targetRot, unitInfo);
        nowSkill.HideRange();
    }

    private void FinishSkill()
    {
        animator.SetBool("IsAttacking", false);
    }

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        unitInfo = GetComponent<UnitInfo>();
    }

    private void Update()
    {
        if (isDead) return;
        if (isClicked)
        {
            percent = Mathf.Clamp(percent + Time.deltaTime * chargeSpeed, 0, maxPercent);
            chargeBar.fillAmount = percent * 0.0078f;
            if(index < 4 && powerGrade[index + 1] < percent)
                index += 1;

            if(index > 0)
            nowSkill.ShowRange(index, transform.position, playerMove.targetRot);
        }
    }

    private void FinishUltimate()
    {
        animator.SetBool("IsUltimating", false);
    }

    public void Death()
    {
        isClicked = false;
    }
}
