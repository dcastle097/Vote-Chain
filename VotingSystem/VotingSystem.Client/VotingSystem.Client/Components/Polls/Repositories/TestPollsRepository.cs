using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Domain.DTOs.Responses;

namespace VotingSystem.Client.Components.Polls.Repositories
{
    public class TestPollsRepository : IPollsRepository
    {
        private readonly IList<PollResponseDto> _polls = new List<PollResponseDto>
        {
            new PollResponseDto
            {
                Address = "2ccfe5fd-a882-4966-88cc-5af5b91f20a4",
                Title = "Reducción de salario",
                Statement =
                    "¿Aprueba usted reducir el salario de los congresistas de 40 a 25 Salarios Mínimos Legales Mensuales Vigentes-SMLMV, fijando un tope de 25 SMLMV como máxima remuneración mensual de los congresistas y altos funcionarios del Estado señalados en el artículo 197 de la Constitución Política?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            },
            new PollResponseDto
            {
                Address = "fcd8b6c7-cee2-4017-b275-b88ca55d2235",
                Title = "Cumplimiento de penas",
                Statement =
                    "¿Aprueba usted que las personas condenadas por corrupción y delitos contra la administración pública deban cumplir la totalidad de las penas en la cárcel, sin posibilidades de reclusión especial, y que el Estado unilateralmente pueda dar por terminados los contratos con ellas y con las personas jurídicas de las que hagan parte, sin que haya lugar a indemnización alguna para el contratista ni posibilidad de volver a contratar con el Estado?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" },
                    new OptionResponseDto { Index = 2, Value = "Option 3" },
                    new OptionResponseDto { Index = 3, Value = "Option 4" },
                    new OptionResponseDto { Index = 4, Value = "Option 5" },
                    new OptionResponseDto { Index = 5, Value = "Option 6" },
                    new OptionResponseDto { Index = 6, Value = "Option 7" },
                    new OptionResponseDto { Index = 7, Value = "Option 8" },
                    new OptionResponseDto { Index = 8, Value = "Option 9" },
                    new OptionResponseDto { Index = 9, Value = "Option 10" },
                    new OptionResponseDto { Index = 10, Value = "Option 11" },
                    new OptionResponseDto { Index = 11, Value = "Option 12" },
                    new OptionResponseDto { Index = 12, Value = "Option 13" },
                    new OptionResponseDto { Index = 13, Value = "Option 14" },
                    new OptionResponseDto { Index = 14, Value = "Option 15" },
                    new OptionResponseDto { Index = 15, Value = "Option 16" }
                }
            },
            new PollResponseDto
            {
                Address = "e3a77b54-fb88-4e20-a1f1-cedb91b89cf3",
                Title = "Contratación transparente",
                Statement =
                    "¿Aprueba usted establecer la obligación a todas las entidades públicas y territoriales de usar pliegos tipo, que reduzcan la manipulación de requisitos habilitantes y ponderables y la contratación a dedo con un número anormalmente bajo de proponentes, en todo tipo de contrato con recursos públicos?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            },
            new PollResponseDto
            {
                Address = "c1f82841-973f-41f4-a57d-21798d39b4b1",
                Title = "Audiencias públicas",
                Statement =
                    "¿Aprueba usted establecer la obligación de realizar audiencias públicas para que la ciudadanía y los corporados decidan el desglose y priorización del presupuesto de inversión de la Nación, los departamentos y los municipios, así como en la rendición de cuentas sobre su contratación y ejecución?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            },
            new PollResponseDto
            {
                Address = "a679ffa2-dcbf-4f1e-a400-f3ae25b54725",
                Title = "Rendición de cuentas",
                Statement =
                    "¿Aprueba usted obligar a congresistas y demás corporados a rendir cuentas anualmente sobre su asistencia, iniciativas presentadas, votaciones, debates, gestión de intereses particulares o de lobbistas, proyectos, partidas e inversiones públicas que haya gestionado y cargos públicos para los cuales hayan presentado candidatos?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            },
            new PollResponseDto
            {
                Address = "b7c72e68-7be2-4a86-b23a-28bf76231e9b",
                Title = "Escrutinio público",
                Statement =
                    "¿Aprueba usted obligar a todos los electos mediante VOTOS popular a hacer público a escrutinio de la ciudadanía sus declaraciones de bienes, patrimonio, rentas, pago de impuestos y conflictos de interés, como requisito para posesionarse y ejercer el cargo; incorporando la facultad de iniciar de oficio investigaciones penales y aplicar la extinción de dominio al elegido y a su potencial red de testaferros como su cónyuge, compañero o compañera permanente, a sus parientes dentro del cuarto grado de consanguinidad, segundo de afinidad y primero civil, y a sus socios de derecho o de hecho?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            },
            new PollResponseDto
            {
                Address = "7a025959-de92-4cf4-b53b-6ee683653c9c",
                Title = "Límite de periodos de elección",
                Statement =
                    "¿Aprueba usted establecer un límite de máximo tres periodos para ser elegido y ejercer en una misma corporación de elección popular como el Senado de la República, la Cámara de Representantes, las Asambleas Departamentales, los Concejos Municipales y las Juntas Administradoras Locales?",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Voted = false,
                Options = new List<OptionResponseDto>
                {
                    new OptionResponseDto { Index = 0, Value = "Option 1" },
                    new OptionResponseDto { Index = 1, Value = "Option 2" }
                }
            }
        };

        public Task<IList<PollResponseDto>> GetPollsAsync(string userId)
        {
            return Task.FromResult(_polls);
        }

        public Task<PollResponseDto> GetPollDetailAsync(string address)
        {
            return Task.FromResult(_polls.FirstOrDefault(poll => poll.Address == address));
        }

        public Task<bool> CastVoteAsync(string pollAddress, byte option, string userAddress, string password)
        {
            throw new NotImplementedException();
        }
    }
}