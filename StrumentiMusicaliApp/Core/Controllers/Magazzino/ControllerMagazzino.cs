using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerMagazzino : BaseController, IDisposable, ICloseSave, IMenu
    {
        public MovimentoMagazzino SelectedItem { get; set; } = new MovimentoMagazzino();

        public const string TAG_SCARICA = "SCARICA";
        public const string TAG_CARICA = "CARICA";
        private Subscription<ScaricaQtaMagazzino> sub1;
        private Subscription<CaricaQtaMagazzino> sub2;
        public ControllerMagazzino()
            : base()
        {

            sub1 = EventAggregator.Instance().Subscribe<ScaricaQtaMagazzino>(ScaricaMagazzino);
            sub2 = EventAggregator.Instance().Subscribe<CaricaQtaMagazzino>(CaricaMagazzino);
        }
        public ControllerMagazzino(Articolo articolo)
            : this()
        {
            ArticoloFilter = articolo;
        }

        public Articolo ArticoloFilter { get; private set; }
        private MenuTab _menuTab = null;
        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                AggiungiComandi();
            }
            return _menuTab;
        }
        private void AggiungiComandi()
        {
            var tabArticoli = _menuTab.Add("Principale");

            var panelComandiArticoli = tabArticoli.Add("Comandi");
            UtilityView.AddButtonSaveAndClose(panelComandiArticoli, this, false);

            var ribCarica = panelComandiArticoli.Add("Carica", StrumentiMusicali.Core.Properties.ImageIcons.Add);
            ribCarica.Tag = TAG_CARICA;
            ribCarica.Click += (a, e) =>
            {
                EventAggregator.Instance().Publish<ValidateViewEvent<Magazzino>>(null);

                EventAggregator.Instance().Publish<CaricaQtaMagazzino>(new CaricaQtaMagazzino()
                {
                    Qta = SelectedItem.Qta,
                    Deposito = SelectedItem.Deposito,
                    ArticoloID = SelectedItem.ArticoloID
                });
            };

            var ribScarica = panelComandiArticoli.Add("Scarica", StrumentiMusicali.Core.Properties.ImageIcons.Remove);
            ribScarica.Tag = TAG_SCARICA;
            ribScarica.Click += (a, e) =>
            {
                EventAggregator.Instance().Publish<ValidateViewEvent<Magazzino>>(null);
                EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(new ScaricaQtaMagazzino()
                {
                    Qta = SelectedItem.Qta,
                    Deposito = SelectedItem.Deposito,
                    ArticoloID = SelectedItem.ArticoloID
                });
            };

        }


        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public override void Dispose()
        {
            base.Dispose();
            EventAggregator.Instance().UnSbscribe(sub1);
            EventAggregator.Instance().UnSbscribe(sub2);

        }
        internal System.Collections.Generic.List<DepositoScaricoItem> ListDepositi()
        {
            using (var uof = new UnitOfWork())
            {
                var listQtaDepositi = uof.MagazzinoRepository.Find(a => a.ArticoloID == SelectedItem.ArticoloID)
                                        .Select(
                                        a => new
                                        {
                                            ID = a.DepositoID,
                                            Qta = a.Qta
                                        }
                                        )
                                        .GroupBy(a => new { a.ID })
                                        .Select(g => new { ID = g.Key.ID, Qta = g.Sum(x => x.Qta) })
                                        .ToList();
                /*mette i depositi vuoti per quell'articolo*/
                var listDepositi = uof.DepositoRepository.Find(a => 1 == 1).ToList()
                    .Distinct().Select(
                    a => new DepositoScaricoItem()
                    {
                        ID = a.ID,
                        NomeDeposito = a.NomeDeposito,
                    }
                    ).ToList();

                foreach (var item in listDepositi)
                {
                    var giac = listQtaDepositi.Where(b => b.ID == item.ID).FirstOrDefault();
                    if (giac != null)
                        item.Qta = giac.Qta;
                }

                listDepositi = listDepositi.Select(a => a).OrderBy(a => a.Descrizione).ToList();
                return listDepositi;
            }
        }

        private void CaricaMagazzino(CaricaQtaMagazzino obj)
        {
            try
            {

                using (var uof = new UnitOfWork())
                {
                    NuovoMovimento(new MovimentoMagazzino()
                    { ArticoloID = obj.ArticoloID, Deposito = obj.Deposito, Qta = obj.Qta }, uof);
                    uof.Commit();
                    MessageManager.NotificaInfo("Aggiunto movimento magazzino");
                    EventAggregator.Instance().Publish<MovimentiUpdate>(new MovimentiUpdate());
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        public bool NuovoMovimento(MovimentoMagazzino movimento, UnitOfWork uof)
        {

            using (var curs = new CursorManager())
            {

                if (movimento.Qta < 0)
                {
                    var qtaDepositata = uof.MagazzinoRepository.Find(a => a.ArticoloID == movimento.ArticoloID &&
                    a.DepositoID == movimento.Deposito).Select(a => a.Qta).DefaultIfEmpty(0).Sum();
                    if (qtaDepositata < Math.Abs(movimento.Qta))
                    {
                        var depositoSel = uof.DepositoRepository.Find(a => a.ID == movimento.Deposito).First();
                        MessageManager.NotificaWarnig(
                            string.Format(
                            @"La quantità presente nel deposito {0} è di {1} pezzi",
                            depositoSel.NomeDeposito,
                            qtaDepositata
                            ));
                        return false;
                    }
                }

                uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
                {
                    ArticoloID = movimento.ArticoloID,
                    DepositoID = movimento.Deposito,
                    Qta = (int)movimento.Qta,
                    PrezzoAcquisto = (int)movimento.Qta>0 ? movimento.Prezzo : 0,
                    
                    Note = movimento.Causale,
                }) ;
                return true;



            }

        }

        private void ScaricaMagazzino(ScaricaQtaMagazzino obj)
        {
            try
            {

                using (var uof = new UnitOfWork())
                {
                    NuovoMovimento(new MovimentoMagazzino()
                    { ArticoloID = obj.ArticoloID, Deposito = obj.Deposito, Qta = -obj.Qta }, uof);
                    
                    uof.Commit();
                    MessageManager.NotificaInfo("Aggiunto movimento magazzino");
                    EventAggregator.Instance().Publish<MovimentiUpdate>(new MovimentiUpdate());
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }

        }

        public void RaiseSave()
        {

        }

        public void RaiseClose()
        {
            OnClose?.Invoke(this, new EventArgs());
        }
    }
}