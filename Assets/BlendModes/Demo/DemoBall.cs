using UnityEngine;
using UnityEngine.EventSystems;

namespace BlendModes
{
	public class DemoBall : MonoBehaviour, IDragHandler
	{
		public void OnDrag (PointerEventData eventData)
		{
			transform.position = eventData.position;
		}
	}
}