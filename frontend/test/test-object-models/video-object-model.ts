import { expect, Locator, Page } from '@playwright/test';

export class VideoObjectModel {
    readonly email = 'creator@test.com';
    readonly password = '123!@#asdASD';
    readonly title = this.getRandomTitle();
    readonly description = 'description';

    readonly page: Page;

    readonly loginLink: Locator;
    readonly addVideoLink: Locator;
    readonly logoutLink: Locator;

    readonly emailInput: Locator;
    readonly passwordInput: Locator;
    readonly submitButton: Locator;
    readonly videoTitleInput: Locator;
    readonly videoDescriptionInput: Locator;
    readonly thumbnailFileUpload: Locator;
    readonly videoFileUpload: Locator;


    constructor(page: Page) {
        this.page = page;
        
        this.loginLink = page.locator('#login-button');
        this.addVideoLink = page.locator('#add-video-button')
        this.logoutLink = page.locator('#logout-button');

        this.emailInput = page.locator('#email-input');
        this.passwordInput = page.locator('#password-input');
        this.submitButton = page.locator('p-button[type="submit"]');
        this.videoTitleInput = page.locator('#video-title-input');
        this.videoDescriptionInput = page.locator('#video-description-input');
        this.thumbnailFileUpload = page.locator('#thumbnail-file-upload');
        this.videoFileUpload = page.locator('#video-file-upload');
    }

    private getRandomTitle() {
        return Math.random().toString(36).substring(2, 15) + 'video';
    }

    async goToHome() {
        await this.page.goto('http://localhost:4200');
    }

    async login() {
        await this.loginLink.click();
        await this.emailInput.fill(this.email);
        await this.passwordInput.fill(this.password);
        await this.submitButton.click();
    }

    async expectLoginSuccess() {
        await expect(this.page).toHaveURL('http://localhost:4200');
        const token = await this.page.evaluate(() => localStorage.getItem('token'));
        expect(token != null).toBeTruthy();
    }

    async addVideo() {
        await this.addVideoLink.click();
        await this.videoTitleInput.fill(this.title);
        await this.videoDescriptionInput.fill(this.description);
        await this.thumbnailFileUpload.setInputFiles('../test-data/thumbnail.png');
        await this.videoFileUpload.setInputFiles('../test-data/video.mp4');
        await this.submitButton.click();
    }

    async expectAddVideoSuccess() {
        // Waiting for search video feature to be implemented
    }

    async logout() {
        await this.logoutLink.click();
    }

    async expectLogoutSuccess() {
        const token = await this.page.evaluate(() => localStorage.getItem('token'));
        expect(token).toBeNull();
    }
}