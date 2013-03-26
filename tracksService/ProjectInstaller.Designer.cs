namespace tracksService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tracksServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.tracksServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // tracksServiceProcessInstaller
            // 
            this.tracksServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.tracksServiceProcessInstaller.Password = null;
            this.tracksServiceProcessInstaller.Username = null;
            // 
            // tracksServiceInstaller
            // 
            this.tracksServiceInstaller.Description = "Tracks REST API Service Host";
            this.tracksServiceInstaller.DisplayName = "TracksService";
            this.tracksServiceInstaller.ServiceName = "TracksService";
            this.tracksServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.tracksServiceProcessInstaller,
            this.tracksServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller tracksServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller tracksServiceInstaller;
    }
}