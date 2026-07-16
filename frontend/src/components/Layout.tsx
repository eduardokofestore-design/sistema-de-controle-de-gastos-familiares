import { NavLink, Outlet } from 'react-router-dom';

export function Layout() {
  return (
    <div className="app-shell">
      <header className="hero-card">
        <div>
          <p className="eyebrow">Controle de gastos</p>
          <h1>Gestão simples e segura</h1>
          <p className="hero-copy">
            Acompanhe pessoas, transações e relatórios com uma navegação clara e segura.
          </p>
        </div>
        <nav className="nav-links" aria-label="Navegação principal">
          <NavLink to="/" end>
            Dashboard
          </NavLink>
          <NavLink to="/pessoas">Pessoas</NavLink>
          <NavLink to="/transacoes">Transações</NavLink>
          <NavLink to="/relatorios">Relatórios</NavLink>
        </nav>
      </header>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
