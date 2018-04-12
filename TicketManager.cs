using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SEP.Movil.Business.Hubs
{
    public class Ticket
    {
        public string Id { get;set; }
        public string AtendidoPor { get; set; }
        public string UsuarioReporte { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public EstadoTicket Status { get; set; }
    }
    public enum EstadoTicket
    {
        Cerrado, Abierto, EnProceso, Descartado
    }
    public class TicketManager
    {
        private List<Ticket> tickets;
        private IHubContext<MesaDeTicketsHub> Hub { get; set; }
        public TicketManager(IHubContext<MesaDeTicketsHub> hub)
        {
            Hub = hub;

            var random = new Random();
            
            tickets = new List<Ticket> {
                new Ticket { Id = random.Next().ToString(), UsuarioReporte = "jatovar", Fecha = DateTime.Now, Motivo = "Nuevo Software" , Status = EstadoTicket.Abierto},
                new Ticket {Id = random.Next().ToString(), UsuarioReporte = "lajuarez", Fecha = DateTime.Now, Motivo = "Nuevo Monitor" , Status = EstadoTicket.Abierto }
            };
        }
        internal async Task BroadcastCambiaEstado(string id, string operador, EstadoTicket estado)
        {
            var ticketFound = tickets.Find(ticket => ticket.Id == id);
            
            ticketFound.Status = estado;
            ticketFound.AtendidoPor = operador;

            switch(estado)
            {
                case EstadoTicket.Cerrado:
                    await Hub.Clients.All.InvokeAsync("ticketCerrado", id , operador);
                break;
                case EstadoTicket.Abierto:
                    await Hub.Clients.All.InvokeAsync("ticketAbierto", id , operador);
                break;
                case EstadoTicket.EnProceso:
                    await Hub.Clients.All.InvokeAsync("ticketEnProceso", id , operador);
                break;
                case EstadoTicket.Descartado:
                    await Hub.Clients.All.InvokeAsync("ticketDescartado", id , operador);
                break;
            }
        }
        public IEnumerable<Ticket> ObtieneTickets()
        {
            return tickets;
        }
        public async Task BroadcastNuevoTicket()
        {
            var random = new Random().Next();

            var ticket = new Ticket {
                Id = random.ToString(),
                UsuarioReporte = "jatovar",
                Fecha = DateTime.Now,
                Status = EstadoTicket.Abierto
            };
            var json = JsonConvert.SerializeObject(ticket);
            await Hub.Clients.All.InvokeAsync("nuevoTicket", json);
        }
    }
}