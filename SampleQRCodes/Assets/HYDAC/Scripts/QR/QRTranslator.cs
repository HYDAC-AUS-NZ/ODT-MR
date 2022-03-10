using System.Collections.Generic;

using UnityEngine;

using QRTracking;

namespace HYDAC.QR
{
    public class QRTranslator : MonoBehaviour
    {
        public int DictionaryCount = 0;
        public GameObject qrCodePrefab;

        [SerializeField] private SocQRCallBacks qrCallbacks;

        [SerializeField] private GameObject qrScanSquarePrefab;

        private System.Collections.Generic.SortedDictionary<System.Guid, GameObject> qrCodesObjectsList;
        private bool clearExisting = false;

        struct ActionData
        {
            public enum Type
            {
                Added,
                Updated,
                Removed
            };
            public Type type;
            public Microsoft.MixedReality.QR.QRCode qrCode;

            public ActionData(Type type, Microsoft.MixedReality.QR.QRCode qRCode) : this()
            {
                this.type = type;
                qrCode = qRCode;
            }
        }

        private System.Collections.Generic.Queue<ActionData> pendingActions = new Queue<ActionData>();

        private bool _isQRRunning;
        private GameObject _qrScanSquare;

        // Use this for initialization
        void Start()
        {
            Debug.Log("QRTranslator start");

            qrCodesObjectsList = new SortedDictionary<System.Guid, GameObject>();

            QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
            QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
            QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
            QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;

            qrCallbacks.EOnQRCodeCreated += OnQrCodeCreated;
            qrCallbacks.EOnQRCodeClosed += OnQRCodeClosed;

            var mainCameraTransform = Camera.main.transform;
            _qrScanSquare = Instantiate(qrScanSquarePrefab, mainCameraTransform);

            OnUIStartTracking();
        }

        private void Instance_QRCodesTrackingStateChanged(object sender, bool status)
        {
            if (!status)
            {
                clearExisting = true;
            }
        }

        private void Instance_QRCodeAdded(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            Debug.Log("#QRTranslator#-----------QRCodeAdded");

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Added, e.Data));
            }
        }

        private void Instance_QRCodeUpdated(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            //Debug.Log("#QRTranslator#-----------QRCodeUpdated");

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Updated, e.Data));
            }

        }

        private void Instance_QRCodeRemoved(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            //Debug.Log("#QRTranslator#-----------QRCodeRemoved");

            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Removed, e.Data));
            }
        }

        private void HandleEvents()
        {
            lock (pendingActions)
            {
                while (pendingActions.Count > 0)
                {
                    var action = pendingActions.Dequeue();
                    if (action.type == ActionData.Type.Added)
                    {
                        GameObject qrCodeObject = Instantiate(qrCodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        qrCodeObject.GetComponent<SpatialGraphNodeTracker>().Id = action.qrCode.SpatialGraphNodeId;
                        qrCodeObject.GetComponent<QRCode>().qrCode = action.qrCode;
                        qrCodesObjectsList.Add(action.qrCode.Id, qrCodeObject);


                    }
                    else if (action.type == ActionData.Type.Updated)
                    {
                        if (!qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {
                            GameObject qrCodeObject = Instantiate(qrCodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                            qrCodeObject.GetComponent<SpatialGraphNodeTracker>().Id = action.qrCode.SpatialGraphNodeId;
                            qrCodeObject.GetComponent<QRCode>().qrCode = action.qrCode;
                            qrCodesObjectsList.Add(action.qrCode.Id, qrCodeObject);
                        }
                    }
                    else if (action.type == ActionData.Type.Removed)
                    {
                        if (qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {
                            Destroy(qrCodesObjectsList[action.qrCode.Id]);
                            qrCodesObjectsList.Remove(action.qrCode.Id);
                        }
                    }
                }
            }
            if (clearExisting)
            {
                clearExisting = false;
                foreach (var obj in qrCodesObjectsList)
                {
                    Destroy(obj.Value);
                }
                qrCodesObjectsList.Clear();

            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (!_isQRRunning)
                return;
            
            HandleEvents();

            DictionaryCount = qrCodesObjectsList.Count;
        }

        private void OnQrCodeCreated(Microsoft.MixedReality.QR.QRCode obj)
        {
            OnUIStopTracking();
        }

        private void OnQRCodeClosed(Microsoft.MixedReality.QR.QRCode qrCode)
        {
            //Debug.Log("#QRTranslator#-----------Deleting QR code");

            if (qrCodesObjectsList.ContainsKey(qrCode.Id))
            {
                Destroy(qrCodesObjectsList[qrCode.Id]);
                qrCodesObjectsList.Remove(qrCode.Id);
            }

            OnUIStartTracking();
        }

        public void OnUIStopTracking()
        {
            _isQRRunning = false;
            
            QRCodesManager.Instance.enabled = false;
            
            _qrScanSquare.SetActive(false);
        }

        public void OnUIStartTracking()
        {
            _isQRRunning = true;

            QRCodesManager.Instance.enabled = true;

            _qrScanSquare.SetActive(true);
        }
    }
}
