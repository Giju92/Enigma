using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour {
    float PageFlipTime = 0;
    public Book ControledBook;
    public int AnimationSpeedFrames = 40;
    bool isFlipping = false;

    // Use this for initialization
    void Start () {
		
        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
	}
	void Update(){
        if (Input.GetButtonDown("Left Page")) {
            FlipLeftPage();            
        }
        if (Input.GetButtonDown("Right Page")) {
            FlipRightPage();
        }
    }

    void PageFlipped()
    {
        isFlipping = false;
    }

	public bool IsFlipping() {
		return isFlipping;
	}
    public void FlipRightPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount-2) return;
        isFlipping = true;
		GetComponent<AudioSource>().Play();
        float frameTime = PageFlipTime / AnimationSpeedFrames;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl)*2 / AnimationSpeedFrames;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }

    public void FlipLeftPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= 2) return;
        isFlipping = true;
		GetComponent<AudioSource>().Play();
        float frameTime = PageFlipTime / AnimationSpeedFrames;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationSpeedFrames;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }
   
    
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationSpeedFrames; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }
    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationSpeedFrames; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
    }
}
