using System;

namespace client
{
    partial class ip_text
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ip_label = new System.Windows.Forms.Label();
            this.iptext = new System.Windows.Forms.TextBox();
            this.porttext = new System.Windows.Forms.TextBox();
            this.port_label = new System.Windows.Forms.Label();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.text_message = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.channell = new System.Windows.Forms.TextBox();
            this.button_subscribe = new System.Windows.Forms.Button();
            this.unsubscribe_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ip_label
            // 
            this.ip_label.AutoSize = true;
            this.ip_label.Location = new System.Drawing.Point(52, 53);
            this.ip_label.Name = "ip_label";
            this.ip_label.Size = new System.Drawing.Size(18, 16);
            this.ip_label.TabIndex = 0;
            this.ip_label.Text = "ip";
            this.ip_label.Click += new System.EventHandler(this.label1_Click);
            // 
            // iptext
            // 
            this.iptext.Location = new System.Drawing.Point(174, 53);
            this.iptext.Name = "iptext";
            this.iptext.Size = new System.Drawing.Size(100, 22);
            this.iptext.TabIndex = 1;
            this.iptext.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // porttext
            // 
            this.porttext.Location = new System.Drawing.Point(174, 102);
            this.porttext.Name = "porttext";
            this.porttext.Size = new System.Drawing.Size(100, 22);
            this.porttext.TabIndex = 2;
            this.porttext.TextChanged += new System.EventHandler(this.porttext_TextChanged);
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(52, 108);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(30, 16);
            this.port_label.TabIndex = 3;
            this.port_label.Text = "port";
            this.port_label.Click += new System.EventHandler(this.label2_Click);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(174, 196);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(512, 53);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(259, 246);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            this.logs.TextChanged += new System.EventHandler(this.logs_TextChanged);
            // 
            // text_message
            // 
            this.text_message.Enabled = false;
            this.text_message.Location = new System.Drawing.Point(138, 365);
            this.text_message.Name = "text_message";
            this.text_message.Size = new System.Drawing.Size(100, 22);
            this.text_message.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 365);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "message";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(316, 362);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 8;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(174, 152);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 22);
            this.textBoxUsername.TabIndex = 9;
            this.textBoxUsername.TextChanged += new System.EventHandler(this.textBoxUsername_TextChanged_1);
            // 
            // channell
            // 
            this.channell.Enabled = false;
            this.channell.Location = new System.Drawing.Point(138, 271);
            this.channell.Name = "channell";
            this.channell.Size = new System.Drawing.Size(100, 22);
            this.channell.TabIndex = 10;
            this.channell.TextChanged += new System.EventHandler(this.channell_TextChanged);
            // 
            // button_subscribe
            // 
            this.button_subscribe.Enabled = false;
            this.button_subscribe.Location = new System.Drawing.Point(301, 243);
            this.button_subscribe.Name = "button_subscribe";
            this.button_subscribe.Size = new System.Drawing.Size(90, 28);
            this.button_subscribe.TabIndex = 11;
            this.button_subscribe.Text = "subscribe";
            this.button_subscribe.UseVisualStyleBackColor = true;
            this.button_subscribe.Click += new System.EventHandler(this.button_subscribe_Click);
            // 
            // unsubscribe_button
            // 
            this.unsubscribe_button.Enabled = false;
            this.unsubscribe_button.Location = new System.Drawing.Point(301, 296);
            this.unsubscribe_button.Name = "unsubscribe_button";
            this.unsubscribe_button.Size = new System.Drawing.Size(90, 27);
            this.unsubscribe_button.TabIndex = 12;
            this.unsubscribe_button.Text = "unsubscribe";
            this.unsubscribe_button.UseVisualStyleBackColor = true;
            this.unsubscribe_button.Click += new System.EventHandler(this.unsubscribe_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "channel name:";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "user name:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // ip_text
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.unsubscribe_button);
            this.Controls.Add(this.button_subscribe);
            this.Controls.Add(this.channell);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_message);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.porttext);
            this.Controls.Add(this.iptext);
            this.Controls.Add(this.ip_label);
            this.Name = "ip_text";
            this.Text = "id";
            this.Load += new System.EventHandler(this.ip_text_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label3_Click(object sender, EventArgs e)
        {
          
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void channell_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBoxUsername_TextChanged_1(object sender, EventArgs e)
        {
             
        }

        private void ip_text_Load(object sender, EventArgs e)
        {
             
        }

        private void logs_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void porttext_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void label1_Click(object sender, EventArgs e)
        {
 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        #endregion

        private System.Windows.Forms.Label ip_label;
        private System.Windows.Forms.TextBox iptext;
        private System.Windows.Forms.TextBox porttext;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox text_message;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox channell;
        private System.Windows.Forms.Button button_subscribe;
        private System.Windows.Forms.Button unsubscribe_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

