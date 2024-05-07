using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PlaneType
    {
        LoginPlane,
        MainPlane,
        Register,
        RegisterPersonalInformation,
        EditorPlane
    }

    [System.Serializable]
    public class PlaneContainer
    {
        public PlaneType planeType;

        public PlaneBase planeBase;
    }

    [Header("[PlaneType]")]
    public List<PlaneContainer> m_planePools = new List<PlaneContainer>();

    private PlaneType  m_PlaneType=PlaneType.Register;

    public static GameManager m_shelf = null;

    public UserData userData;

    private void Awake()
    {
        m_shelf = this;

        CutPlane(PlaneType.LoginPlane);
    }

    /// <summary>
    ///«–ªªUI√Ê∞Â
    /// </summary>
    /// <param name="planeType"></param>
    /// <returns></returns>
    public  PlaneBase CutPlane(PlaneType planeType)
    {
        PlaneContainer planeContainer  = m_planePools.Find((obj => obj.planeType == planeType));

        PlaneContainer planeContainerOld= m_planePools.Find((obj => obj.planeType == m_PlaneType));

        if (planeContainerOld != null)
         planeContainerOld.planeBase.ExitPlane();

        planeContainer.planeBase.EnterPlane();

        return planeContainer.planeBase;
    }

    /// <summary>
    /// ªÒ»°Plane
    /// </summary>
    /// <param name="planeType"></param>
    /// <returns></returns>
    public PlaneBase GetPlane(PlaneType planeType)
    {
        PlaneContainer planeContainer = m_planePools.Find((obj => obj.planeType == planeType));

        return planeContainer.planeBase;
    }
}
