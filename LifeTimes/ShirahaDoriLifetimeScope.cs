using VContainer;
using VContainer.Unity;
using UnityEngine;


namespace yumehiko.ShirahaDori
{
    public class ShirahaDoriLifetimeScope : LifetimeScope
    {
        [SerializeField] private RoundUI roundCallUI;
        [SerializeField] private DistanceView distanceView;
        [SerializeField] private KatanaDamageView katanaDamageView;
        [SerializeField] private ShirahaDoriAnimation shirahaDoriAnimation;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ShirahaDoriPresenter>(Lifetime.Singleton);
            builder.RegisterComponent(shirahaDoriAnimation);

            builder.Register<RoundPresenter>(Lifetime.Singleton);
            builder.Register<Round>(Lifetime.Singleton);
            builder.RegisterComponent(roundCallUI);

            builder.Register<KatanaCatchPresenter>(Lifetime.Singleton);
            builder.Register<Distance>(Lifetime.Singleton);
            builder.RegisterComponent(distanceView);

            builder.Register<KatanaDamagePresenter>(Lifetime.Singleton);
            builder.Register<KatanaDamage>(Lifetime.Singleton);
            builder.RegisterComponent(katanaDamageView);
        }
    }
}