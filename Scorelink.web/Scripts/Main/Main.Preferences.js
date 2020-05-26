// *************************************************************
// Copyright (c) 1991-2019 LEAD Technologies, Inc.
// All Rights Reserved.
// *************************************************************
var HTML5Demos;
(function (HTML5Demos) {
    var DocumentViewerDemo;
    (function (DocumentViewerDemo) {
        // Contains the preferences part and demo settings
        var PreferencesPart = /** @class */ (function () {
            function PreferencesPart(main) {
                // Reference to the DocumentViewerDemoApp
                this._mainApp = null;
                this.headerToolbar_PreferencesMenu = {
                    preferencesMenuItem: "#preferencesMenuItem",
                    userNameMenuItem: "#userNameMenuItem",
                    showTextIndicatorsMenuItem: "#showTextIndicators",
                    showLinksMenuItem: "#showLinks",
                    documentViewerOptionsMenuItem: "#documentViewerOptions",
                };
                this._mainApp = main;
                this.preferredItemType = lt.Document.Viewer.DocumentViewerItemType.image;
                if (this._mainApp.demoMode != DocumentViewerDemo.DemoMode.Barcode)
                    this.showTextIndicators = true;
                this.showLinks = false;
                this.enableInertiaScroll = true;
                this.initPreferencesUI();
            }
            PreferencesPart.prototype.initPreferencesUI = function () {
                $(this.headerToolbar_PreferencesMenu.preferencesMenuItem).on("click", this.preferencesMenuItem_Click.bind(this));
                $(this.headerToolbar_PreferencesMenu.userNameMenuItem).on("click", this.userNameMenuItem_Click.bind(this));
                $(this.headerToolbar_PreferencesMenu.showTextIndicatorsMenuItem).on("click", this.showTextIndicatorsMenuItem_Click.bind(this));
                $(this.headerToolbar_PreferencesMenu.showLinksMenuItem).on("click", this.showLinksMenuItem_Click.bind(this));
                $(this.headerToolbar_PreferencesMenu.documentViewerOptionsMenuItem).on("click", this.documentViewerOptionsMenuItem_Click.bind(this));
            };
            PreferencesPart.prototype.preferencesMenuItem_Click = function (e) {
                lt.Demos.Utils.UI.toggleChecked($(this.headerToolbar_PreferencesMenu.showTextIndicatorsMenuItem).find(".icon"), this.showTextIndicators);
                lt.Demos.Utils.UI.toggleChecked($(this.headerToolbar_PreferencesMenu.showLinksMenuItem).find(".icon"), this.showLinks);
            };
            PreferencesPart.prototype.userNameMenuItem_Click = function (e) {
                var _this = this;
                var inputDlg = this._mainApp.inputDlg;
                inputDlg.showWith("User Name", "Enter the user name for modifying annotations in the document.", this._mainApp.documentViewer.userName, false, false);
                inputDlg.onApply = function (userName) {
                    if (userName) {
                        _this._mainApp.documentViewer.userName = userName;
                        lt.Demos.Annotations.AutomationObjectsListControl.userName = userName;
                    }
                    else {
                        _this._mainApp.documentViewer.userName = "Author";
                        lt.Demos.Annotations.AutomationObjectsListControl.userName = "Author";
                    }
                    return true;
                };
            };
            PreferencesPart.prototype.showTextIndicatorsMenuItem_Click = function (e) {
                this.showTextIndicators = !this.showTextIndicators;
                lt.Demos.Utils.UI.toggleChecked($(this.headerToolbar_PreferencesMenu.showTextIndicatorsMenuItem).find(".icon"), this.showTextIndicators);
                // Invalidate the view
                this._mainApp.documentViewer.view.invalidate(lt.LeadRectD.empty);
                if (this._mainApp.documentViewer.thumbnails != null)
                    this._mainApp.documentViewer.thumbnails.invalidate(lt.LeadRectD.empty);
            };
            PreferencesPart.prototype.showLinksMenuItem_Click = function (e) {
                this.showLinks = !this.showLinks;
                lt.Demos.Utils.UI.toggleChecked($(this.headerToolbar_PreferencesMenu.showLinksMenuItem).find(".icon"), this.showLinks);
                this._mainApp.documentViewer.diagnostics.showLinks = this.showLinks;
            };
            PreferencesPart.prototype.documentViewerOptionsMenuItem_Click = function (e) {
                // Set the documentViewer for the dialog
                var dlg = this._mainApp.documentViewerOptionsDlg;
                var app = this._mainApp;
                dlg.hookPrepareAjax = app.hookPrepareAjax;
                dlg.useCSSTransitions = app.useCSSTransitions;
                dlg.useStatusQueryRequests = app.useStatusQueryRequests;
                dlg.verifyUploadedMimeTypes = app.verifyUploadedMimeTypes;
                dlg.loadDocumentMode = app.loadDocumentMode;
                if (app.serviceHeartbeat) {
                    dlg.heartbeatEnabled = app.serviceHeartbeat.isStarted;
                    dlg.heartbeatStart = app.serviceHeartbeat.startTimeout;
                    dlg.heartbeatInterval = app.serviceHeartbeat.interval;
                    dlg.heartbeatAutoPause = app.serviceHeartbeat.autoPauseInteractionTimeout;
                }
                dlg.documentViewer = this._mainApp.documentViewer;
                dlg.hasUserToken = this._mainApp.hasUserToken;
                dlg.show();
            };
            return PreferencesPart;
        }());
        DocumentViewerDemo.PreferencesPart = PreferencesPart;
    })(DocumentViewerDemo = HTML5Demos.DocumentViewerDemo || (HTML5Demos.DocumentViewerDemo = {}));
})(HTML5Demos || (HTML5Demos = {}));
