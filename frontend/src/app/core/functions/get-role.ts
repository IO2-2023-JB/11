export function getRole(): string {
    const role = sessionStorage.getItem('role');
    if (role === null) {
      return '';
    }
    return role;
  }
  