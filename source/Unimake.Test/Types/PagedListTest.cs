using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Unimake.Primitives.Collections.Page;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Types
{
    public class PagedListTest
    {
        #region Public Classes

        public class Content
        {
            #region Public Properties

            [JsonProperty("Banco")]
            public long Banco { get; set; }

            [JsonProperty("Header")]
            public Header Header { get; set; }

            [JsonProperty("Nome")]
            public string Nome { get; set; }

            #endregion Public Properties
        }

        public class Header
        {
            #region Public Properties

            [JsonProperty("Agencia")]
            public long Agencia { get; set; }

            [JsonProperty("Banco")]
            public long Banco { get; set; }

            [JsonProperty("CodigoAgenciaBeneficiario")]
            public long CodigoAgenciaBeneficiario { get; set; }

            [JsonProperty("CodigoDaRemessa")]
            public long CodigoDaRemessa { get; set; }

            [JsonProperty("CodigoDeServico")]
            public long CodigoDeServico { get; set; }

            [JsonProperty("CodigoDoBeneficiario")]
            public long CodigoDoBeneficiario { get; set; }

            [JsonProperty("Conta")]
            public long Conta { get; set; }

            [JsonProperty("ContaCobranca")]
            public long ContaCobranca { get; set; }

            [JsonProperty("DAC")]
            public long Dac { get; set; }

            [JsonProperty("DataCredito")]
            public DateTimeOffset DataCredito { get; set; }

            [JsonProperty("DataGeracaoArquivo")]
            public string DataGeracaoArquivo { get; set; }

            [JsonProperty("Densidade")]
            public long Densidade { get; set; }

            [JsonProperty("Geracao")]
            public DateTimeOffset Geracao { get; set; }

            [JsonProperty("InscricaoBeneficiario")]
            public object InscricaoBeneficiario { get; set; }

            [JsonProperty("NomeDaEmpresa")]
            public string NomeDaEmpresa { get; set; }

            [JsonProperty("Numero")]
            public long Numero { get; set; }

            [JsonProperty("NumeroSequenciaArquivo")]
            public long NumeroSequenciaArquivo { get; set; }

            [JsonProperty("Operacao")]
            public long Operacao { get; set; }

            [JsonProperty("Reservado1")]
            public object Reservado1 { get; set; }

            [JsonProperty("Reservado2")]
            public object Reservado2 { get; set; }

            [JsonProperty("Reservado3")]
            public object Reservado3 { get; set; }

            [JsonProperty("Servico")]
            public long Servico { get; set; }

            [JsonProperty("SiglaEmpresaNoSistema")]
            public object SiglaEmpresaNoSistema { get; set; }

            [JsonProperty("Tipo")]
            public long Tipo { get; set; }

            [JsonProperty("UnidadeDensidade")]
            public string UnidadeDensidade { get; set; }

            #endregion Public Properties
        }

        public class Item
        {
            #region Public Properties

            [JsonProperty("Content")]
            public Content Content { get; set; }

            [JsonProperty("Identifier")]
            public string Identifier { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes

        #region Public Methods

        [Fact]
        public void GetFromJson()
        {
            var json = "{\"Items\":[{\"Content\":{\"Banco\":341,\"Header\":{\"DataCredito\":\"2020-07-23\",\"Densidade\":1600,\"InscricaoBeneficiario\":null,\"Numero\":2105,\"UnidadeDensidade\":\"BPI\",\"Agencia\":999,\"Banco\":341,\"CodigoAgenciaBeneficiario\":0,\"CodigoDaRemessa\":0,\"CodigoDeServico\":0,\"CodigoDoBeneficiario\":0,\"Conta\":999,\"ContaCobranca\":0,\"DAC\":5,\"DataGeracaoArquivo\":\"\",\"Geracao\":\"2020-07-23\",\"NomeDaEmpresa\":\"Lorem ipsum dolor\",\"Operacao\":2,\"Reservado1\":null,\"Reservado2\":null,\"Reservado3\":null,\"Servico\":1,\"SiglaEmpresaNoSistema\":null,\"Tipo\":0,\"NumeroSequenciaArquivo\":0},\"Nome\":\"Lorem Ipsum dolor\"},\"Identifier\":\"79338219000155_COB_341_00999_240720_00.RET\"}],\"PageInfo\":{\"CurrentPage\":1,\"Filtered\":true,\"HasNext\":false,\"HasPrevious\":false,\"ItemsCount\":1,\"PageSize\":10,\"TotalCount\":1,\"TotalPages\":1},\"RecordInfo\":{\"PageCount\":37273,\"PageSize\":10,\"RecordCount\":372730}}";
            var paged = JsonConvert.DeserializeObject<PagedList<Item>>(json);
            var list = paged.Items;

            foreach(var item in paged)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
            }

            paged = new PagedList<Item>(null, paged.PageInfo, paged.RecordInfo);

            foreach(var item in paged)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
            }

            paged = new PagedList<Item>(list, paged.PageInfo, paged.RecordInfo);

            foreach(var item in paged)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
            }
        }

        #endregion Public Methods
    }
}