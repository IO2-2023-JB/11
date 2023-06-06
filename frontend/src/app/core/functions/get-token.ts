export function getToken(): string {
  const token = sessionStorage.getItem('token');
  if (token === null) {
    return '';
  }
  return token;
}
