import type { ReactNode } from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import { RouteGuard } from './auth/RouteGuard';
import { AccessDeniedPage, LoginPage, PasswordRecoveryPage, RegistrationPage } from './auth/LoginPage';
import { AuthProfilePage } from './auth/ProfilePage';
import { SettingsPage } from './auth/SettingsPage';
import { DemoPage } from './auth/DemoPage';
import { DashboardLayout } from './components/layout';
import { AchievementsPage, AdminMissionsPage, AdminPage, AdminSettingsPage, AdminUsersPage, DashboardPage, HelpPage, MissionDetailPage, MissionsPage, NotFoundPage, ProgressPage, ResourcesPage, TeamPage, TeacherMissionsPage, TeacherPage, TeacherReportsPage, TeacherStudentsPage } from './pages';
const student = (page: ReactNode) => <RouteGuard roles={['student']}>{page}</RouteGuard>;
const teacher = (page: ReactNode) => <RouteGuard roles={['teacher']}>{page}</RouteGuard>;
const admin = (page: ReactNode) => <RouteGuard roles={['admin']}>{page}</RouteGuard>;
export default function App() { return <Routes>
  <Route path="/login" element={<LoginPage />} /><Route path="/registro" element={<RegistrationPage />} /><Route path="/recuperar-contrasena" element={<PasswordRecoveryPage />} /><Route path="/demo" element={<DemoPage />} /><Route path="/acceso" element={<Navigate to="/login" replace />} /><Route path="/acceso-denegado" element={<AccessDeniedPage />} />
  <Route path="*" element={<DashboardLayout><Routes>
    <Route path="/" element={student(<DashboardPage />)} /><Route path="/misiones" element={student(<MissionsPage />)} /><Route path="/misiones/:id" element={student(<MissionDetailPage />)} /><Route path="/progreso" element={student(<ProgressPage />)} /><Route path="/logros" element={student(<AchievementsPage />)} /><Route path="/perfil" element={<RouteGuard><AuthProfilePage /></RouteGuard>} /><Route path="/configuracion" element={<RouteGuard><SettingsPage /></RouteGuard>} />
    <Route path="/recursos" element={<ResourcesPage />} /><Route path="/equipo" element={<TeamPage />} /><Route path="/ayuda" element={<HelpPage />} />
    <Route path="/docente" element={teacher(<TeacherPage />)} /><Route path="/docente/estudiantes" element={teacher(<TeacherStudentsPage />)} /><Route path="/docente/misiones" element={teacher(<TeacherMissionsPage />)} /><Route path="/docente/reportes" element={teacher(<TeacherReportsPage />)} />
    <Route path="/admin" element={admin(<AdminPage />)} /><Route path="/admin/usuarios" element={admin(<AdminUsersPage />)} /><Route path="/admin/misiones" element={admin(<AdminMissionsPage />)} /><Route path="/admin/configuracion" element={admin(<AdminSettingsPage />)} /><Route path="*" element={<NotFoundPage />} />
  </Routes></DashboardLayout>} />
</Routes>; }
