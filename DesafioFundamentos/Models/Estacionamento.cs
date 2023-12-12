using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DesafioFundamentos.Models
{
    
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private List<string> veiculos = new List<string>();

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        public void AdicionarVeiculo()
        {
            //carregando data json.
            var arquivoJson = "Data/data.json";
            var json = File.ReadAllText(arquivoJson);

            //coletando valores a serem gerados na data
            Console.WriteLine("Digite a placa do veículo para estacionar:");
            string placa = Console.ReadLine();
            Console.WriteLine("Digite o modelo do veículo para estacionar:");
            string modelo = Console.ReadLine();
            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            //convertendo valores em uma array com key placa
            JArray carro = new JArray();
            carro.Add($"modelo: {modelo.ToLower()}");
            carro.Add($"date: {date.ToLower()}");

            JObject o = new JObject();
            o[placa.ToLower()] = carro;

            //adicionando novo veiculo
            var jsonObj = JObject.Parse(json);
            var arrayAuto = jsonObj.GetValue("data") as JArray;
            var newAuto = JObject.Parse(o.ToString());
            arrayAuto.Add(newAuto);

            //passando valores da nova matriz para data json.
            jsonObj["data"] = arrayAuto;
            string novoJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(arquivoJson, novoJsonResult);
        }
        public void ConsultarVeiculo()
        {
            //carregando data json.
            var arquivoJson = "Data/data.json";
            var json = File.ReadAllText(arquivoJson);

            //coletando valores a serem colsultados no caso a placa
            Console.WriteLine("Digite a placa do veículo para consultar:");
            string placaConsult = Console.ReadLine();
            var consult = JObject.Parse(json);

            JToken consultplca = consult.SelectToken( $"$.data[?(@.{placaConsult})]" );
            string modelo = ( string )consultplca[placaConsult][ 0 ];
            string date = ( string )consultplca[placaConsult][ 1 ];
            Console.WriteLine($"PLACA: {placaConsult.ToUpper()}\n{modelo.ToUpper()}\n{date.ToUpper()}");
            
        }
        public void RemoverVeiculo()
        {
            //carregando data json.
            var arquivoJson = "Data/data.json";
            var json = File.ReadAllText(arquivoJson);

            // Converte o JSON para um objeto JObject
            JObject jsonObj = JObject.Parse(json);

            // Obtém a matriz 'data' como um JArray
            JArray placas = (JArray)jsonObj["data"];

            // Critério para encontrar o objeto a ser removido (por exemplo, pelo nome)
            Console.WriteLine("Digite a placa do veículo para remover:");
            string placaConsult = Console.ReadLine().ToLower();
            //var consult = JObject.Parse(json);

            JToken consultplca = jsonObj.SelectToken( $"$.data[?(@.{placaConsult})]" );

            string modelo = ( string )consultplca[placaConsult][ 0 ];
            string date = ( string )consultplca[placaConsult][ 1 ];

            string placaParaRemover = consultplca.ToString();

            // Encontra o índice do objeto com base no critério
            int indiceParaRemover = -1;
            for (int i = 0; i < placas.Count; i++)
            {
                if (placas[i].ToString() == placaParaRemover)
                {
                    indiceParaRemover = i;
                    break;
                }
            }

            // Remove o objeto se encontrado
            if (indiceParaRemover != -1)
            {
                placas.RemoveAt(indiceParaRemover);

                // Converte o JObject de volta para uma string JSON
                string novoJson = jsonObj.ToString();
                var dataAtualizada = JObject.Parse(novoJson);
                string novoJsonResult = JsonConvert.SerializeObject(dataAtualizada, Formatting.Indented);
                File.WriteAllText(arquivoJson, novoJsonResult);

                // Exibe o novo JSON3
                
                Console.WriteLine("Digite quantas hora(s) o carro ficou no estacionamento.");
                int horas = Convert.ToInt32(Console.ReadLine());
                decimal valorTotal = 0; 

                valorTotal = precoInicial + precoPorHora * horas;
                // TODO: Remover a placa digitada da lista de veículos
                // *IMPLEMENTE AQUI*

                Console.WriteLine($"O veículo {placaConsult} foi removido e o preço total foi de: R$ {valorTotal}");
            }
            else
            {
                Console.WriteLine($"O objeto com '{placaParaRemover}' não foi encontrado na matriz.");
            }
                
            
        }
        public void ListarVeiculos()
        {
            //carregando data json.
            var arquivoJson = "Data/data.json";
            var json = File.ReadAllText(arquivoJson);

            // Converte o JSON para um objeto JObject
            JObject jsonObj = JObject.Parse(json);

            // Obtém a matriz 'data' como um JArray
            JArray placas = (JArray)jsonObj["data"];
            // Verifica se há veículos no estacionamento
            if (placas.Any())
            {
                Console.WriteLine("Os veículos estacionados são:");
                // TODO: Realizar um laço de repetição, exibindo os veículos estacionados
                // *IMPLEMENTE AQUI*

                foreach (var placa in placas)
                {
                    Console.WriteLine(placa);
                }
            }
            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }
    }
}
