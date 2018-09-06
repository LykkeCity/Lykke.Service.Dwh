using System;
using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator;
using Lykke.HttpClientGenerator.Infrastructure;

namespace Lykke.Service.Dwh.Client
{
    [PublicAPI]
    public static class AutofacExtensions
    {
        /// <summary>
        /// Registers <see cref="IDwhClient"/> in Autofac container using <see cref="DwhServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">LykkeService client settings.</param>
        /// <param name="builderConfigure">Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.</param>
        public static void RegisterLykkeServiceClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] DwhServiceClientSettings settings,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(DwhServiceClientSettings.ServiceUrl));

            RegisterLykkeServiceClient(
                builder,
                settings?.ServiceUrl,
                builderConfigure);
        }

        /// <summary>
        /// Registers <see cref="IDwhClient"/> in Autofac container using <see cref="DwhServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="serviceUrl">LykkeService url.</param>
        /// <param name="builderConfigure">Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.</param>
        public static void RegisterLykkeServiceClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] string serviceUrl,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentNullException(nameof(serviceUrl));

            var clientBuilder = HttpClientGenerator.HttpClientGenerator.BuildForUrl(serviceUrl)
                .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper());

            clientBuilder = builderConfigure?.Invoke(clientBuilder) ?? clientBuilder.WithoutRetries();

            builder.RegisterInstance(clientBuilder.Create().Generate<IDwhClient>())
                .As<IDwhClient>()
                .SingleInstance();
        }
    }
}
