#pragma warning disable 0067, 0108
// ------------------------------------
// 
// Assembly MockingFrameworksCompare
// 
// ------------------------------------
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of Brain</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = Brain")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SBrain
      : global::MockingFrameworksCompare.BrainSample.Brain
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SBrain</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SBrain()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of BurnException</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = BurnException")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SBurnException
      : global::MockingFrameworksCompare.BrainSample.BurnException
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SBurnException</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SBurnException()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.ShoppingCartSample.Stubs
{
    /// <summary>Stub of ContactDetails</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ContactDetails")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SContactDetails
      : global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SContactDetails</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SContactDetails()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of property MockingFrameworksCompare.ShoppingCartSample.ContactDetails.Name</summary>
        public override string Name
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh = this.NameGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  if (this.callBase)
                    return base.Name;
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
                  return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                    .SContactDetails, string>(this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh = this.NameSet
                  ;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
                  sh.Invoke(value);
                else 
                {
                  if (this.callBase)
                    base.Name = value;
                  else 
                  {
                    global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                      ;
                    stub.VoidResult<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                      .SContactDetails>(this);
                  }
                }
            }
        }

        /// <summary>Stub of method System.String MockingFrameworksCompare.ShoppingCartSample.ContactDetails.get_Name()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> NameGet;

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ContactDetails.set_Name(System.String value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> NameSet;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of Hand</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = Hand")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SHand
      : global::MockingFrameworksCompare.BrainSample.Hand
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SHand</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SHand()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of IHand</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IHand")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIHand
      : global::Microsoft.Stubs.Framework.StubBase
      , global::MockingFrameworksCompare.BrainSample.IHand
    {
        /// <summary>Initializes a new instance of type SIHand</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIHand()
        {
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.BrainSample.IHand.TouchIron(MockingFrameworksCompare.BrainSample.Iron iron)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::MockingFrameworksCompare.BrainSample.IHand.TouchIron(global::MockingFrameworksCompare.BrainSample.Iron iron)
        {
            global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::MockingFrameworksCompare.BrainSample.Iron> sh = this.TouchIron;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::MockingFrameworksCompare.BrainSample.Iron>)null)
              sh.Invoke(iron);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::MockingFrameworksCompare.BrainSample.Stubs.SIHand>
                  (this);
            }
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.BrainSample.IHand.TouchIron(MockingFrameworksCompare.BrainSample.Iron iron)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::MockingFrameworksCompare.BrainSample.Iron> TouchIron;
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of IMouth</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IMouth")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIMouth
      : global::Microsoft.Stubs.Framework.StubBase
      , global::MockingFrameworksCompare.BrainSample.IMouth
    {
        /// <summary>Initializes a new instance of type SIMouth</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIMouth()
        {
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.BrainSample.IMouth.Yell()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::MockingFrameworksCompare.BrainSample.IMouth.Yell()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action sh = this.Yell;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action)null)
              sh.Invoke();
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::MockingFrameworksCompare.BrainSample.Stubs.SIMouth>
                  (this);
            }
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.BrainSample.IMouth.Yell()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action Yell;
    }
}
namespace MockingFrameworksCompare.ShoppingCartSample.Stubs
{
    /// <summary>Stub of IWarehouse</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IWarehouse")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIWarehouse
      : global::Microsoft.Stubs.Framework.StubBase
      , global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse
    {
        /// <summary>Initializes a new instance of type SIWarehouse</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIWarehouse()
        {
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;MockingFrameworksCompare.ShoppingCartSample.Product&gt; MockingFrameworksCompare.ShoppingCartSample.IWarehouse.GetProducts(System.String name)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string, global::System.Collections.Generic.List<global::MockingFrameworksCompare.ShoppingCartSample.Product>> GetProducts;

        /// <summary>Stub of property MockingFrameworksCompare.ShoppingCartSample.IWarehouse.IsAvailable</summary>
        bool global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse.IsAvailable
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<bool> sh
                   = this.IsAvailableGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<bool>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                    .SIWarehouse, bool>(this);
                }
            }
        }

        /// <summary>Stub of method System.Boolean MockingFrameworksCompare.ShoppingCartSample.IWarehouse.get_IsAvailable()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<bool> IsAvailableGet;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;MockingFrameworksCompare.ShoppingCartSample.Product&gt; MockingFrameworksCompare.ShoppingCartSample.IWarehouse.GetProducts(System.String name)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.Collections.Generic.List<global::MockingFrameworksCompare.ShoppingCartSample.Product> global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse.GetProducts(string name)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::MockingFrameworksCompare.
                ShoppingCartSample.Product>> sh = this.GetProducts;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::MockingFrameworksCompare.
                ShoppingCartSample.Product>>)null)
              return sh.Invoke(name);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                .SIWarehouse, global::System.Collections.Generic.List<
              global::MockingFrameworksCompare.ShoppingCartSample
                .Product>>(this);
            }
        }

        /// <summary>Event SomethingWentWrong</summary>
        public global::System.EventHandler<global::MockingFrameworksCompare.ShoppingCartSample.WarehouseEventArgs> SomethingWentWrong;

        event global::System.EventHandler<global::MockingFrameworksCompare.ShoppingCartSample.WarehouseEventArgs> global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse.SomethingWentWrong
        {
            [global::System.Diagnostics.DebuggerHidden]
            add
            {
                this.SomethingWentWrong = (global::System
                  .EventHandler<global::MockingFrameworksCompare.ShoppingCartSample
                    .WarehouseEventArgs>)(global::System.Delegate.Combine
                    ((global::System.Delegate)(this.SomethingWentWrong), 
                    (global::System.Delegate)value));
            }
            [global::System.Diagnostics.DebuggerHidden]
            remove
            {
                this.SomethingWentWrong = (global::System
                  .EventHandler<global::MockingFrameworksCompare.ShoppingCartSample
                    .WarehouseEventArgs>)(global::System.Delegate.Remove
                    ((global::System.Delegate)(this.SomethingWentWrong), 
                    (global::System.Delegate)value));
            }
        }
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of Iron</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = Iron")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIron
      : global::MockingFrameworksCompare.BrainSample.Iron
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SIron</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIron()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.BrainSample.Stubs
{
    /// <summary>Stub of Mouth</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = Mouth")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SMouth
      : global::MockingFrameworksCompare.BrainSample.Mouth
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SMouth</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SMouth()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.ShoppingCartSample.Stubs
{
    /// <summary>Stub of ShoppingCart</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ShoppingCart")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SShoppingCart
      : global::MockingFrameworksCompare.ShoppingCartSample.ShoppingCart
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SShoppingCart</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SShoppingCart()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.AddProducts(System.String name, MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void AddProducts(string name, global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action
                <string, global::MockingFrameworksCompare.ShoppingCartSample
                  .IWarehouse> sh = this.AddProductsStringIWarehouse;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string, 
            global::MockingFrameworksCompare.ShoppingCartSample
              .IWarehouse>)null)
              sh.Invoke(name, warehouse);
            else 
            {
              if (this.callBase)
                base.AddProducts(name, warehouse);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                  .SShoppingCart>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.AddProductsIfWarehouseAvailable(System.String name, MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void AddProductsIfWarehouseAvailable(string name, global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action
                <string, global::MockingFrameworksCompare.ShoppingCartSample
                  .IWarehouse> sh = this.AddProductsIfWarehouseAvailableStringIWarehouse;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string, 
            global::MockingFrameworksCompare.ShoppingCartSample
              .IWarehouse>)null)
              sh.Invoke(name, warehouse);
            else 
            {
              if (this.callBase)
                base.AddProductsIfWarehouseAvailable(name, warehouse);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                  .SShoppingCart>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.AddProductsIfWarehouseAvailable(System.String name, MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string, global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse> AddProductsIfWarehouseAvailableStringIWarehouse;

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.AddProducts(System.String name, MockingFrameworksCompare.ShoppingCartSample.IWarehouse warehouse)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string, global::MockingFrameworksCompare.ShoppingCartSample.IWarehouse> AddProductsStringIWarehouse;

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.Int32 MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.GetProductsCount()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override int GetProductsCount()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<int> sh
               = this.GetProductsCount01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<int>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.GetProductsCount();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                .SShoppingCart, int>(this);
            }
        }

        /// <summary>Stub of method System.Int32 MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.GetProductsCount()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<int> GetProductsCount01;

        /// <summary>Stub of property MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.IsRed</summary>
        public override bool IsRed
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<bool> sh = this.IsRedGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<bool>)null)
                  return sh.Invoke();
                else 
                {
                  if (this.callBase)
                    return base.IsRed;
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
                  return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                    .SShoppingCart, bool>(this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Action<bool> sh = this.IsRedSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<bool>)null)
                  sh.Invoke(value);
                else 
                {
                  if (this.callBase)
                    base.IsRed = value;
                  else 
                  {
                    global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                      ;
                    stub.VoidResult<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                      .SShoppingCart>(this);
                  }
                }
            }
        }

        /// <summary>Stub of method System.Boolean MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.get_IsRed()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<bool> IsRedGet;

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.set_IsRed(System.Boolean value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<bool> IsRedSet;

        /// <summary>Stub of property MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.User</summary>
        public override global::MockingFrameworksCompare.ShoppingCartSample.User User
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates
                  .Func<global::MockingFrameworksCompare.ShoppingCartSample.User> sh
                   = this.UserGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<
                global::MockingFrameworksCompare.ShoppingCartSample.User
                >)null)
                  return sh.Invoke();
                else 
                {
                  if (this.callBase)
                    return base.User;
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
                  return stub.Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                    .SShoppingCart, 
                  global::MockingFrameworksCompare.ShoppingCartSample.User>(this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates
                  .Action<global::MockingFrameworksCompare.ShoppingCartSample.User> sh
                   = this.UserSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<
                global::MockingFrameworksCompare.ShoppingCartSample.User
                >)null)
                  sh.Invoke(value);
                else 
                {
                  if (this.callBase)
                    base.User = value;
                  else 
                  {
                    global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                      ;
                    stub.VoidResult<global::MockingFrameworksCompare.ShoppingCartSample.Stubs
                      .SShoppingCart>(this);
                  }
                }
            }
        }

        /// <summary>Stub of method MockingFrameworksCompare.ShoppingCartSample.User MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.get_User()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::MockingFrameworksCompare.ShoppingCartSample.User> UserGet;

        /// <summary>Gets an instance of TStub and assigns it as the delegate of get_User</summary>
        public TStub UserGetAsStub<TStub>()
            where TStub : global::MockingFrameworksCompare.ShoppingCartSample.User, new()
        {
            this.UserGet = global::Microsoft.Stubs.Framework.StubExtensions
              .GetAsStub<global::MockingFrameworksCompare.ShoppingCartSample.User, TStub>
                (this._UserGetAsStub, this.UserGet, out this._UserGetAsStub);
            return (TStub)(this._UserGetAsStub);
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.ShoppingCart.set_User(MockingFrameworksCompare.ShoppingCartSample.User value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::MockingFrameworksCompare.ShoppingCartSample.User> UserSet;

        private global::MockingFrameworksCompare.ShoppingCartSample.User _UserGetAsStub;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.ShoppingCartSample.Stubs
{
    /// <summary>Stub of User</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = User")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SUser
      : global::MockingFrameworksCompare.ShoppingCartSample.User
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SUser</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SUser()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Stub of property MockingFrameworksCompare.ShoppingCartSample.User.ContactDetails</summary>
        public override global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails ContactDetails
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates
                  .Func<global::MockingFrameworksCompare.ShoppingCartSample
                    .ContactDetails> sh = this.ContactDetailsGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
                  .Func<global::MockingFrameworksCompare.ShoppingCartSample
                    .ContactDetails>)null)
                  return sh.Invoke();
                else 
                {
                  if (this.callBase)
                    return base.ContactDetails;
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
                  return stub
                    .Result<global::MockingFrameworksCompare.ShoppingCartSample.Stubs.SUser, 
                    global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails>(this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates
                  .Action<global::MockingFrameworksCompare.ShoppingCartSample
                    .ContactDetails> sh = this.ContactDetailsSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
                  .Action<global::MockingFrameworksCompare.ShoppingCartSample
                    .ContactDetails>)null)
                  sh.Invoke(value);
                else 
                {
                  if (this.callBase)
                    base.ContactDetails = value;
                  else 
                  {
                    global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                      ;
                    stub.VoidResult
                        <global::MockingFrameworksCompare.ShoppingCartSample.Stubs.SUser>(this);
                  }
                }
            }
        }

        /// <summary>Stub of method MockingFrameworksCompare.ShoppingCartSample.ContactDetails MockingFrameworksCompare.ShoppingCartSample.User.get_ContactDetails()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails> ContactDetailsGet;

        /// <summary>Gets an instance of TStub and assigns it as the delegate of get_ContactDetails</summary>
        public TStub ContactDetailsGetAsStub<TStub>()
            where TStub : global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails, new()
        {
            this.ContactDetailsGet =
              global::Microsoft.Stubs.Framework.StubExtensions.GetAsStub
                  <global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails, 
                  TStub>(this._ContactDetailsGetAsStub, 
              this.ContactDetailsGet, out this._ContactDetailsGetAsStub);
            return (TStub)(this._ContactDetailsGetAsStub);
        }

        /// <summary>Stub of method System.Void MockingFrameworksCompare.ShoppingCartSample.User.set_ContactDetails(MockingFrameworksCompare.ShoppingCartSample.ContactDetails value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails> ContactDetailsSet;

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private global::MockingFrameworksCompare.ShoppingCartSample.ContactDetails _ContactDetailsGetAsStub;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace MockingFrameworksCompare.ShoppingCartSample.Stubs
{
    /// <summary>Stub of WarehouseEventArgs</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.12.40430.3")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = WarehouseEventArgs")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SWarehouseEventArgs
      : global::MockingFrameworksCompare.ShoppingCartSample.WarehouseEventArgs
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SWarehouseEventArgs</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SWarehouseEventArgs()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}

