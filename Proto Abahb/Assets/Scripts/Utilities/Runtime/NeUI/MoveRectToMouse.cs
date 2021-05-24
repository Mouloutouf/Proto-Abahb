using UnityEngine;

namespace Utilities.Runtime.NeUI
{
    public class MoveRectToMouse : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rect;
        [SerializeField]
        private RectTransform parent;
        // Start is called before the first frame update
        void Start()
        {
            if(!rect) TryGetComponent(out rect);
            if(!parent) transform.parent.TryGetComponent(out parent);
        }

        public virtual void MoveToMouse()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, Camera.current,
                out Vector2 localPoint);
            rect.anchoredPosition = localPoint;
        }
    }
}
