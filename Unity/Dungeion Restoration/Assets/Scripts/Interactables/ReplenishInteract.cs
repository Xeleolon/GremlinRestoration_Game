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
    Slider bar;
    public Gradient gradient;
    Image fill;
    Transform playerHead;
    public float speed = 1.0f;
    void Start()
    {
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
            Completed(3);
            string message = new string(gameObject.name + " Replenish it supples");
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
        ReplinishCanvas = Instantiate(sliderPrefab, transform.position, Quaternion.identity);
        bar = ReplinishCanvas.transform.Find("Slider").GetComponent<Slider>();
        fill = bar.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();

        bar.maxValue = maxValue;
        bar.value = 0;
    }
    public override void Completed(int type)
    {
        base.Completed(type);
    }

}
