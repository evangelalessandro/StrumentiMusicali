using StrumentiMusicali.App.View.Attributes;

namespace StrumentiMusicali.App.View.Enums
{
	public enum enAmbiente
	{
		[UIAmbienteAttribute(true)]
		Main=1,
		[UIAmbienteAttribute(true)]
		Fattura=2,
		[UIAmbienteAttribute(false)]
		FattureList=3,
		[UIAmbienteAttribute(true)]
		Articolo=4,
		[UIAmbienteAttribute(true)]
		ArticoloSconto = 700,
		[UIAmbienteAttribute(false)]
		ArticoliList=5,
		Magazzino=6,
		[UIAmbienteAttribute(false)]
		SettingFatture=7,
		[UIAmbienteAttribute(false)]
		SettingSito=8,
		[UIAmbienteAttribute(false)]
		SettingStampa=9,
		[UIAmbienteAttribute(false)]
		ScaricoMagazzino=10,
		//[UIAmbienteAttribute(false)]
		//LogView=11,
		[UIAmbienteAttribute(false)]
		LogViewList=12,
		[UIAmbienteAttribute(false)]
		ClientiList=13,
		[UIAmbienteAttribute(true)]
		Cliente=14,
		FattureRigheList=15,
		[UIAmbienteAttribute(true)]
		FattureRigheDett=16,
		[UIAmbienteAttribute(true)]
		Deposito=17,
		[UIAmbienteAttribute(false)]
		DepositoList=18,

		[UIAmbienteAttribute(true)]
		Utente = 20,
		[UIAmbienteAttribute(false)]
		UtentiList = 21,



		NonSpecificato = 100,
        [UIAmbienteAttribute(false)]
        PagamentiList = 701,
        [UIAmbienteAttribute(true)]
        Pagamento = 702,
    }


}