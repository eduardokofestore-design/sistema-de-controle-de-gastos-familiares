import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css';
import './App.css';
import { Layout } from './components/Layout';
import { DashboardPage } from './pages/DashboardPage';
import { PessoasPage } from './pages/PessoasPage';
import { TransacoesPage } from './pages/TransacoesPage';
import { RelatoriosPage } from './pages/RelatoriosPage';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/pessoas" element={<PessoasPage />} />
          <Route path="/transacoes" element={<TransacoesPage />} />
          <Route path="/relatorios" element={<RelatoriosPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>,
);
