namespace server_project
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_listen = new Button();
            all_clients = new RichTextBox();
            if_clients = new RichTextBox();
            sps_clients = new RichTextBox();
            textBox_port = new TextBox();
            label1 = new Label();
            textBox_message = new RichTextBox();
            label2 = new Label();
            send_if = new Button();
            send_sps = new Button();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // button_listen
            // 
            button_listen.Location = new Point(214, 16);
            button_listen.Name = "button_listen";
            button_listen.Size = new Size(94, 29);
            button_listen.TabIndex = 0;
            button_listen.Text = "Listen";
            button_listen.UseVisualStyleBackColor = true;
            button_listen.Click += button_listen_Click;
            // 
            // all_clients
            // 
            all_clients.Location = new Point(12, 62);
            all_clients.Name = "all_clients";
            all_clients.Size = new Size(331, 376);
            all_clients.TabIndex = 2;
            all_clients.Text = "";
            // 
            // if_clients
            // 
            if_clients.Location = new Point(358, 41);
            if_clients.Name = "if_clients";
            if_clients.Size = new Size(202, 168);
            if_clients.TabIndex = 3;
            if_clients.Text = "";
            // 
            // sps_clients
            // 
            sps_clients.Location = new Point(358, 247);
            sps_clients.Name = "sps_clients";
            sps_clients.Size = new Size(202, 191);
            sps_clients.TabIndex = 4;
            sps_clients.Text = "";
            // 
            // textBox_port
            // 
            textBox_port.Location = new Point(83, 16);
            textBox_port.Name = "textBox_port";
            textBox_port.Size = new Size(125, 27);
            textBox_port.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 18);
            label1.Name = "label1";
            label1.Size = new Size(37, 20);
            label1.TabIndex = 6;
            label1.Text = "port";
            // 
            // textBox_message
            // 
            textBox_message.Location = new Point(585, 141);
            textBox_message.Name = "textBox_message";
            textBox_message.Size = new Size(203, 68);
            textBox_message.TabIndex = 7;
            textBox_message.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(585, 109);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 8;
            label2.Text = "message";
            // 
            // send_if
            // 
            send_if.Enabled = false;
            send_if.Location = new Point(631, 230);
            send_if.Name = "send_if";
            send_if.Size = new Size(94, 29);
            send_if.TabIndex = 9;
            send_if.Text = "Send IF";
            send_if.UseVisualStyleBackColor = true;
            send_if.Click += send_if_Click;
            // 
            // send_sps
            // 
            send_sps.Enabled = false;
            send_sps.Location = new Point(631, 282);
            send_sps.Name = "send_sps";
            send_sps.Size = new Size(94, 29);
            send_sps.TabIndex = 10;
            send_sps.Text = "Send SPS";
            send_sps.UseVisualStyleBackColor = true;
            send_sps.Click += send_sps_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(358, 16);
            label3.Name = "label3";
            label3.Size = new Size(59, 20);
            label3.TabIndex = 11;
            label3.Text = "IF 100 : ";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(358, 224);
            label4.Name = "label4";
            label4.Size = new Size(64, 20);
            label4.TabIndex = 12;
            label4.Text = "SPS 101:";
            label4.Click += label4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(send_sps);
            Controls.Add(send_if);
            Controls.Add(label2);
            Controls.Add(textBox_message);
            Controls.Add(label1);
            Controls.Add(textBox_port);
            Controls.Add(sps_clients);
            Controls.Add(if_clients);
            Controls.Add(all_clients);
            Controls.Add(button_listen);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Button button_listen;
        private RichTextBox all_clients;
        private RichTextBox if_clients;
        private RichTextBox sps_clients;
        private TextBox textBox_port;
        private Label label1;
        private RichTextBox textBox_message;
        private Label label2;
        private Button send_if;
        private Button send_sps;
        private Label label3;
        private Label label4;
    }
}