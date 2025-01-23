using esnafagelir_mobilweb.DataAccessLayer;
using Microsoft.EntityFrameworkCore.Storage;

#region Interface
public interface IUnitOfWork : IDisposable
{
    ILoginRepo Login { get; }
    Task<int> SaveChangesAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
#endregion

public class UnitOfWork : IUnitOfWork
{
    private readonly DataBaseContext _context;

    #region Tablolar
    public ILoginRepo Login { get; private set; }
    #endregion

    #region  Ctor
    public UnitOfWork(DataBaseContext context,
                      ILoginRepo loginRepo)
    {
        _context = context;
        Login = loginRepo;

    }
    #endregion

    #region  Metodlar
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
    public async Task CommitTransactionAsync()
    {
        var currentTransaction = _context.Database.CurrentTransaction;
        if (currentTransaction != null)
        {
            await currentTransaction.CommitAsync();
        }
    }
    public async Task RollbackTransactionAsync()
    {
        var currentTransaction = _context.Database.CurrentTransaction;
        if (currentTransaction != null)
        {
            await currentTransaction.RollbackAsync();
        }
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    #endregion
}

