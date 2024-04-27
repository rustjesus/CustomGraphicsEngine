using System;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Microsoft.SqlServer.Server;
using System.Drawing;
using System.IO;

namespace CustomGraphicsEngine
{
    public partial class Form1 : Form
    {
        // Direct3D Components
        private SharpDX.Direct3D11.Device device; // Specified namespace for Device
        private SwapChain swapChain;
        private DeviceContext deviceContext;
        private RenderTargetView renderTargetView;

        public Form1()
        {
            InitializeComponent();
            InitDevice();
            Application.Idle += Application_Idle;
        }

        private void InitDevice()
        {
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(this.ClientSize.Width, this.ClientSize.Height, new Rational(60, 1), SharpDX.DXGI.Format.R8G8B8A8_UNorm), // Specified namespace for Format
                IsWindowed = true,
                OutputHandle = this.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            SharpDX.Direct3D11.Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.None, desc, out device, out swapChain); // Specified namespace for Device creation
            deviceContext = device.ImmediateContext;
            using (var backBuffer = swapChain.GetBackBuffer<Texture2D>(0))
            {
                renderTargetView = new RenderTargetView(device, backBuffer);
            }

            deviceContext.Rasterizer.SetViewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            deviceContext.OutputMerger.SetRenderTargets(renderTargetView);
        }

        private void Render()
        {
            deviceContext.ClearRenderTargetView(renderTargetView, new RawColor4(0, 0.5f, 0.5f, 1));
            swapChain.Present(0, PresentFlags.None);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Render();
        }

        protected override void OnClosed(EventArgs e)
        {
            renderTargetView.Dispose();
            swapChain.Dispose();
            device.Dispose();
            base.OnClosed(e);
        }


    }

}