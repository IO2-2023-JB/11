export function getUserId(): string {
    return sessionStorage.getItem('userId')!;
}
