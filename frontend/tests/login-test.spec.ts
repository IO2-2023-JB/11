import { test, expect } from '@playwright/test';

test('Login-Logout', async ({ page }) => {
  // Login
  await page.goto('http://localhost:4200/login');

  await page.fill('#email-input', 'test@mail.com');
  await page.fill('#password-input', '123!@#asdASD');
  await page.click('#login-button');

  await expect(page).toHaveURL('http://localhost:4200');
  let token = await page.evaluate(() => localStorage.getItem('token'));
  expect(token != null).toBeTruthy();

  // Logout
  await page.click('#logout-button');

  await expect(page).toHaveURL('http://localhost:4200/login');
  token = await page.evaluate(() => localStorage.getItem('token'));
  expect(token).toBeNull();
});
