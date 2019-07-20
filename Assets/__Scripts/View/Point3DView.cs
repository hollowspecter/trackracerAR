/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class Point3DView : MonoBehaviour
{
    public bool IsDirty { get; set; }
    public bool IsCopied { get; set; }

    [SerializeField]
    protected Material m_material;
    [SerializeField]
    protected Color m_unselectedColor = Color.white;
    [SerializeField]
    protected Color m_selectedColor = Color.magenta;

    private Vector3 m_screenPoint;
    private Vector3 m_offset;
    private SignalBus m_signalBus;
    private ObservableLongPointerDownTrigger m_longTapTrigger;
    
    private CompositeDisposable m_subscriptions;
    private Subject<Unit> m_mouseDownAsObservable;

    [Inject]
    private void Construct(SignalBus _signalBus )
    {
        m_signalBus = _signalBus;
    }

    private void Awake()
    {
        m_mouseDownAsObservable = new Subject<Unit> ();
        m_material = GetComponent<MeshRenderer> ().material;
        m_material.color = m_unselectedColor;
        m_longTapTrigger = gameObject.AddComponent<ObservableLongPointerDownTrigger> ();
        IsDirty = false;
    }

    private void OnEnable()
    {
        m_subscriptions = new CompositeDisposable ();
        m_subscriptions.Add(m_longTapTrigger
            .OnLongPointerDownAsObservable ()
            .Subscribe (_ => {
                IsDirty = true;
                m_signalBus.Fire<FeaturePointChanged> ();
            }));

        m_subscriptions.Add (m_mouseDownAsObservable.Buffer (TimeSpan.FromMilliseconds (600))
            .Where (xs => xs.Count >= 2)
            .Subscribe (_ => {
                IsCopied = true;
                m_signalBus.Fire<FeaturePointChanged> ();
            }));
        
    }

    private void OnDisable()
    {
        m_subscriptions?.Dispose ();
    }

    private void OnDestroy()
    {
        m_mouseDownAsObservable.OnCompleted ();
    }

    private void OnMouseDown()
    {
        m_mouseDownAsObservable.OnNext (Unit.Default);
        Select ();
        m_screenPoint = Camera.main.WorldToScreenPoint ( gameObject.transform.position );
        m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint ( new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z ) );
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z );
        Vector3 curPosition = Camera.main.ScreenToWorldPoint ( curScreenPoint ) + m_offset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        Deselect ();
        m_signalBus.Fire ( new FeaturePointChanged () );
    }

    public void Select()
    {
        m_material.color = m_selectedColor;
    }

    public void Deselect()
    {
        m_material.color = m_unselectedColor;
    }
}
