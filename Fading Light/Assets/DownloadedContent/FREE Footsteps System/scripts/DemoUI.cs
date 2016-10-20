// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\DemoUI.cs
//
// summary:	Implements the demo user interface class

using UnityEngine;
using UnityEngine.UI;

namespace Footsteps {

    /// <summary>   A demo user interface. </summary>
    ///
 

	public class DemoUI : MonoBehaviour {

        /// <summary>   Gets the top down controller. </summary>
        ///
        /// <value> The top down controller. </value>

		[SerializeField] GameObject topDownController;

        /// <summary>   Gets the top down camera. </summary>
        ///
        /// <value> The top down camera. </value>

		[SerializeField] GameObject topDownCamera;

        /// <summary>   Gets the first person controller. </summary>
        ///
        /// <value> The first person controller. </value>

		[SerializeField] GameObject firstPersonController;

        /// <summary>   Activates the top down. </summary>
        ///
     

		public void ActivateTopDown() {
			if(!topDownController.activeSelf) topDownController.transform.position = firstPersonController.transform.position;

			firstPersonController.SetActive(false);
			topDownController.SetActive(true);
			topDownCamera.SetActive(true);
		}

        /// <summary>   Activates the first person. </summary>
        ///
     

		public void ActivateFirstPerson() {
			if(!firstPersonController.activeSelf) firstPersonController.transform.position = topDownController.transform.position;

			firstPersonController.SetActive(true);
			topDownController.SetActive(false);
			topDownCamera.SetActive(false);
		}
	}
}
