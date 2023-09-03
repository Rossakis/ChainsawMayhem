using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ChainsawMan
{
    /// <summary>
    /// A class that activates a certain button when the gameObject you add this class to needs a controller to select a UI button, after the gameObject is first activated 
    /// </summary>
    public class ButtonSelect : MonoBehaviour
    {
        public Button firstButtonToBeSelected;//the first button to be selected when Death Screen is activated (mostly for controller support)
        
        void Start()
        {
            firstButtonToBeSelected.Select();
        }

        /// <summary>
        /// Select a button
        /// </summary>
        public void SelectButton()
        {
            firstButtonToBeSelected.Select();
        }
    }
}
