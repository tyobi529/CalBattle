using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
    [DisallowMultipleComponent]
    public class BookUI : MonoBehaviour
    {
        //ページ数
        [SerializeField] GameObject bookLayout;
        //public int pageNum { get; set; }
        int pageNum;

        [SerializeField] GameObject[] buttonObj = new GameObject[3];





        //メニュー一覧
        public GameObject[] dishList = new GameObject[3];

        [SerializeField, Range(0, 3)]
        float TurnTime = 0.5f;
        [SerializeField, Range(-2, 2)]
        float TurnPageTilt = 1f;
        [SerializeField]
        Shader shader;
        [SerializeField]
        public UnityEvent OnPageChanged = new UnityEvent();


        float time = 0f;

        int PageID;
        float _currentPosition = 0;
        float CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
                material.SetFloat(PageID, _currentPosition);
            }
        }

        int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value) return;
                _currentPage = value;
                startPosition = CurrentPosition;
                currentTime = 0;
                if (OnPageChanged != null)
                    OnPageChanged.Invoke();
            }
        }

        Material _mat;
        Material material
        {
            get
            {
                if (_mat == null)
                {
                    _mat = new Material(shader);
                    var matrix = Matrix4x4.Scale(new Vector3(1f / Resolution.x, 1f / Resolution.y, 1)) * CalcCanvas2LocalMatrix();
                    _mat.SetMatrix("_Canvas2Local", matrix);
                    _mat.SetMatrix("_Local2Canvas", matrix.inverse);
                    _mat.SetFloat("_Tilt", TurnPageTilt);
                }
                return _mat;
            }
        }

        Matrix4x4 CalcCanvas2LocalMatrix()
        {
            var canvaslist = GetComponentsInParent<Canvas>();
            return transform.worldToLocalMatrix * canvaslist[canvaslist.Length - 1].transform.worldToLocalMatrix.inverse;
        }

        Vector2? _resolution = null;
        public Vector2 Resolution
        {
            get
            {
                if (_resolution == null) _resolution = CalcResolution();
                return _resolution.Value;
            }
        }

        Vector2 CalcResolution()
        {
            var scaler = GetComponent<CanvasScaler>();
            if (scaler != null)
                if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize)
                {
                    var canvas = GetComponent<Canvas>();
                    if (canvas.isRootCanvas)
                        return scaler.referenceResolution;
                }
            var rect = GetComponent<RectTransform>().rect;
            return new Vector2(rect.width, rect.height);
        }

        void Awake()
        {
   //         PageID = Shader.PropertyToID("_Page");
			//foreach (var g in GetComponentsInChildren<Graphic>(true))
   //             g.material = material;

            //pageNum = bookLayout.transform.childCount;

            //PageChanged();
        }

        public void CountPageNum()
        {
            PageID = Shader.PropertyToID("_Page");
            foreach (var g in GetComponentsInChildren<Graphic>(true))
                g.material = material;

            pageNum = bookLayout.transform.childCount;

            PageChanged();
        }

        float currentTime = -1;
        float startPosition;
        void Update()
        {

            if (currentTime < 0) return;
            currentTime += Time.unscaledDeltaTime;
            float t = currentTime / TurnTime;
            if (currentTime >= TurnTime)
            {
                currentTime = -1;
                t = 1f;
            }
            CurrentPosition = Mathf.SmoothStep(startPosition, CurrentPage, t);

        }

        public void OnPreviousButton()
        {

            for (int i = 0; i < 3; i++)
            {
                dishList[i].SetActive(false);
            }

            StopAllCoroutines();

            if (CurrentPage != 0)
            {
                CurrentPage--;
            }

        }

        public void OnNextButton()
        {

            for (int i = 0; i < 3; i++)
            {
                dishList[i].SetActive(false);
            }

            StopAllCoroutines();

            if (CurrentPage < pageNum - 1)
            {
                CurrentPage++;
            }

        }

        public void OnToFrontButton()
        {
            for (int i = 0; i < 3; i++)
            {
                dishList[i].SetActive(false);
            }

            StopAllCoroutines();

            CurrentPage = 0;
        }


        public void OnToListButton()
        {
            if (CurrentPage - 3 <= 18)
            {
                CurrentPage = 2;
            }
            else
            {
                CurrentPage = 3;
            }
        }

        public void PageChanged()
        {
            if (CurrentPage != 0 && CurrentPage != pageNum - 1)
            {
                RecipeSoundManager.instance.BookSE();

            }



            if (CurrentPage == 1 || CurrentPage == 2 || CurrentPage == 3)
            {
                StartCoroutine(ShowDishList(CurrentPage - 1));
            }

            if (CurrentPage == 0)
            {
                buttonObj[0].SetActive(false);
            }
            else if (CurrentPage == pageNum - 1)
            {
                buttonObj[1].SetActive(false);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    buttonObj[i].SetActive(true);
                }
            }

            if (CurrentPage > 3 && CurrentPage < pageNum)
            {
                buttonObj[2].SetActive(true);
            }
            else
            {
                buttonObj[2].SetActive(false);
            }

        }


        IEnumerator ShowDishList(int pageNum)
        {
            yield return new WaitForSeconds(0.5f);
            dishList[pageNum].SetActive(true);

        }


    }
}
