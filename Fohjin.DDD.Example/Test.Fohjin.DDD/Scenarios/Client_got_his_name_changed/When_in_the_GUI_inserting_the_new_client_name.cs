﻿using System;
using System.Collections.Generic;
using Fohjin.DDD.BankApplication.Presenters;
using Fohjin.DDD.BankApplication.Views;
using Fohjin.DDD.Reporting;
using Fohjin.DDD.Reporting.Dto;
using Moq;

namespace Test.Fohjin.DDD.Scenarios.Client_got_his_name_changed
{
    public class When_in_the_GUI_inserting_the_new_client_name : PresenterTestFixture<ClientDetailsPresenter>
    {
        private readonly Guid _clientId = Guid.NewGuid();
        private ClientDetailsReport _clientDetailsReport;
        private List<ClientDetailsReport> _clientDetailsReports;

        protected override void SetupDependencies()
        {
            _clientDetailsReport = new ClientDetailsReport(_clientId, "Client Name", "Street", "123", "5000", "Bergen", "1234567890");
            _clientDetailsReports = new List<ClientDetailsReport> { _clientDetailsReport };
            OnDependency<IReportingRepository>()
                .Setup(x => x.GetByExample<ClientDetailsReport>(It.IsAny<object>()))
                .Returns(_clientDetailsReports);
        }

        protected override void Given()
        {
            Presenter.SetClient(new ClientReport(_clientId, "Client Name"));
            Presenter.Display();
            On<IClientDetailsView>().ValueFor(x => x.ClientName).IsSetTo("Client name");
            On<IClientDetailsView>().ValueFor(x => x.PhoneNumber).IsSetTo("1234567890");
            On<IClientDetailsView>().ValueFor(x => x.Street).IsSetTo("Street");
            On<IClientDetailsView>().ValueFor(x => x.StreetNumber).IsSetTo("123");
            On<IClientDetailsView>().ValueFor(x => x.PostalCode).IsSetTo("5000");
            On<IClientDetailsView>().ValueFor(x => x.City).IsSetTo("Bergen");
            On<IClientDetailsView>().FireEvent(x => x.OnInitiateClientNameChange += null);
        }

        protected override void When()
        {
            On<IClientDetailsView>().ValueFor(x => x.ClientName).IsSetTo("New Client name");
            On<IClientDetailsView>().FireEvent(x => x.OnFormElementGotChanged += null);
        }

        [Then]
        public void Then_the_save_button_will_be_disabled()
        {
            On<IClientDetailsView>().VerifyThat.Method(x => x.DisableSaveButton()).WasCalled();
        }

        [Then]
        public void Then_the_menu_buttons_will_be_enabled()
        {
            On<IClientDetailsView>().VerifyThat.Method(x => x.EnableAddNewAccountMenu()).WasCalled();
            On<IClientDetailsView>().VerifyThat.Method(x => x.EnableClientHasMovedMenu()).WasCalled();
            On<IClientDetailsView>().VerifyThat.Method(x => x.EnableNameChangedMenu()).WasCalled();
            On<IClientDetailsView>().VerifyThat.Method(x => x.EnablePhoneNumberChangedMenu()).WasCalled();
        }

        [Then]
        public void Then_the_save_button_will_be_enabled()
        {
            On<IClientDetailsView>().VerifyThat.Method(x => x.EnableSaveButton()).WasCalled();
        }
    }
}