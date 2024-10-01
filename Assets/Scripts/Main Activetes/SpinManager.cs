using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using CustomEventBus;
using TMPro;

public class SpinManager : MonoBehaviour
{
   [SerializeField] private Button uiSpinButton;
   [SerializeField] private Button _backButton;
   [SerializeField] private Text uiSpinButtonText;
   [SerializeField] private PickerWheel pickerWheel;
   [SerializeField] private GameObject WinPanel;
   [Header("Texts")]
   [SerializeField] private TMP_Text _textOfMoneyWin;
   [SerializeField] private TMP_Text _textOfMoney;
   [SerializeField] private TMP_Text _textOfFreeSpin;
   private int _freeSpins;
   private int _minimumForStart = 600;

   private void Awake()
   {
      _textOfFreeSpin.text = $"{Iinstance.instance.FreeSpins}";
      EventBus.FreeSpin = StartFortune;
   }
   private void StartFortune(bool WasNotMoney)
   {
      _freeSpins = Iinstance.instance.FreeSpins;
      if (WasNotMoney == true)
         Spin();
      else if (_freeSpins > 0)
      {
         Spin();
         _freeSpins--;
         EventBus.AddFreeSpin.Invoke(-1);
         _textOfFreeSpin.text = $"{Iinstance.instance.FreeSpins}";
      }
      else if (_minimumForStart <= EventBus.GetCoins.Invoke())
      {
         EventBus.SetCoins.Invoke(-_minimumForStart);
         Spin();
      }
   }

   private void Spin()
   {
      _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
      uiSpinButton.interactable = false;
      _backButton.interactable = false;
      uiSpinButtonText.text = "Spinning";

      pickerWheel.OnSpinEnd(wheelPiece =>
      {
         uiSpinButton.interactable = true;
         _backButton.interactable = true;
         uiSpinButtonText.text = "Spin";
         WinPanel.SetActive(true);
         _textOfMoneyWin.text = $"{wheelPiece.Amount} Stars";
         EventBus.SetCoins.Invoke(wheelPiece.Amount);
         _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
         EventBus.UpdateMoney.Invoke();
      });
      pickerWheel.Spin();
   }
}
