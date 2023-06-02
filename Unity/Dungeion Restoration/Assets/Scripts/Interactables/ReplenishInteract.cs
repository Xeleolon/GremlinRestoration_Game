using UnityEngine;
using UnityEngine.UI;

public class ReplenishInteract : Interactable
{
    [Header("Replinish Variables")]
    public GameObject sliderPrefab;
    private GameObject ReplinishCanvas;
    public float maxValue;
    public float fillSpeed = 0.1f;
    private float fillValue = 0;
    public float sliderOffSet = 0;
    Slider bar;
    public Gradient gradient;
    Image fill;
    Transform playerHead;
    public float speed = 1.0f;
    public override void Start()
    {
        acheiveGoal.type = 3;
        base.Start();
        playerHead = GameObject.FindWithTag("MainCamera").transform;

    }
    void Update()
    {
        if (ReplinishCanvas != null)
        {
            //Transform Ui To Face Player
            Vector3 targetRotation = playerHead.position - ReplinishCanvas.transform.position;
            float step = speed * Time.deltaTime;
            Vector3 newRotation = Vector3.RotateTowards(ReplinishCanvas.transform.forward, targetRotation, step, 0.0f);
            ReplinishCanvas.transform.rotation = Quaternion.LookRotation(newRotation);

            bar.value = Mathf.Lerp(bar.value, fillValue, Time.deltaTime);
            fill.color = gradient.Evaluate(bar.normalizedValue);

            if (fillValue >= (maxValue * 2))
            {
                Destroy(ReplinishCanvas);
            }
            else
            {
                fillValue += fillSpeed * Time.deltaTime;
            }
        }
    }
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 3)
        {
            PlayAnimator();
            ActivateSlider();
            Completed();
            string message = new string(gameObject.name + " is Replenished");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
        }
        else
        {
            string message = new string("Beep Boop Bop");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
        }
    }
    public override void PlayAnimator()
    {
        base.PlayAnimator();
    }
    private void ActivateSlider()
    {
        Vector3 slidePosition = transform.localScale;
        slidePosition.x = transform.position.x;
        slidePosition.z = transform.position.z;
        if (sliderOffSet == 0)
        {
            slidePosition.y += transform.position.y;
        }
        else
        {
            slidePosition.y = transform.position.y + sliderOffSet;
        }
        

        ReplinishCanvas = Instantiate(sliderPrefab, slidePosition, Quaternion.identity);
        bar = ReplinishCanvas.transform.Find("Slider").GetComponent<Slider>();
        fill = bar.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();

        bar.maxValue = maxValue;
        bar.value = 0;
    }
    public override void Completed()
    {
        base.Completed();
    }

}
