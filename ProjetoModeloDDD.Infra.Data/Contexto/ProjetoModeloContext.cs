using ProjetoModeloDDD.Domain.Entities;
using ProjetoModeloDDD.Infra.Data.EntityConfig;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ProjetoModeloDDD.Infra.Data.Contexto
{
    public class ProjetoModeloContext : DbContext
    {
        public ProjetoModeloContext() : base("ProjetoModeloDDD")
        {
            
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //remove a pluralização para nomes de tabelas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //remove a exclusão em cascata de relacionamentos um para muitos
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //remove a exclusão em cascata de relacionamentos muitos para muitos
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //toda propriedade que tiver Id no final será uma primary key
            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.IsKey());

            //altera toda propriedade que for string para varchar no banco de dados
            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            //por padrão toda propriedade string terá um tamanho de 100 caracteres
            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));

            modelBuilder.Configurations.Add(new ClienteConfiguration());
            modelBuilder.Configurations.Add(new ProdutoConfiguration());
        }

        public override int SaveChanges()
        {
            //antes de salvar procura por uma propriedade chamada DataCadastro
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                //se o objeto estiver no estado Added, seta a propriedade DataAtual com a data atual
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                //se o objeto estiver no estado Modified, não modifica a propriedade DataCadastro
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChanges();
        }
    }
}
