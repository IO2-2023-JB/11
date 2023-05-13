import { test } from '@playwright/test';
import { RegisterObjectModel } from '../test-object-models/register-object-model';

test('Register', async ({ page }) => {

  const loginObjectModel = new RegisterObjectModel(page);
  await loginObjectModel.goToHome();

  await loginObjectModel.login();
  await loginObjectModel.expectLoginFail();

  await loginObjectModel.login();
  await loginObjectModel.expectLoginSuccess();

  await loginObjectModel.logout();
  await loginObjectModel.expectLogoutSuccess();
});
