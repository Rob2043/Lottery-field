using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using CustomEventBus;
using TMPro;

public class Demo : MonoBehaviour
{
   [SerializeField] private Button uiSpinButton;
   [SerializeField] private Text uiSpinButtonText;

   [SerializeField] private PickerWheel pickerWheel;
   [SerializeField] private GameObject WinPanel;
   [SerializeField] private TMP_Text _textOfMoneyWin;


   private void Start()
   {
      uiSpinButton.onClick.AddListener(() =>
      {

         uiSpinButton.interactable = false;
         uiSpinButtonText.text = "Spinning";

         pickerWheel.OnSpinEnd(wheelPiece =>
         {
            Debug.Log(
               @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
               + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
            );
            uiSpinButton.interactable = true;
            uiSpinButtonText.text = "Spin";
            WinPanel.SetActive(true);
            _textOfMoneyWin.text = $"{wheelPiece.Amount}";
            EventBus.SetCoins.Invoke(wheelPiece.Amount);
         });

         pickerWheel.Spin();

      });

   }

}
