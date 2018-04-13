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
            var index = tickets.IndexOf(ticketFound);
            tickets[index].Status = estado;
            tickets[index].AtendidoPor = operador;

            switch(estado)
            {
                case EstadoTicket.Cerrado:
                    tickets.Remove(ticketFound);
                break;
                case EstadoTicket.Descartado:
                    tickets.Remove(ticketFound);
                break;
                case EstadoTicket.Abierto:
                    tickets[index].AtendidoPor = String.Empty;

                break;
            }
            await Hub.Clients.All.InvokeAsync("refrescaTickets", tickets);
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
                Motivo = "Problema en pc",
                Status = EstadoTicket.Abierto
            };
            tickets.Add(ticket);
            await Hub.Clients.All.InvokeAsync("refrescaTickets", tickets);
        }
    }
}