import carMainSource from "./CarMain.vue?raw";

describe("CarMain.vue", () => {
  it("keeps Edit Car field order aligned with Add Car", () => {
    const editDialogStart = carMainSource.indexOf("<!-- Edit Car Dialog -->");
    const editDialogTemplate = carMainSource.slice(editDialogStart);
    const cardTextStart = editDialogTemplate.indexOf("<v-card-text>");
    const cardTextEnd = editDialogTemplate.indexOf("</v-card-text>");
    const fieldsSection = editDialogTemplate.slice(cardTextStart, cardTextEnd);

    const yearIndex = fieldsSection.indexOf('v-model="editCar.year"');
    const makeIndex = fieldsSection.indexOf('v-model="editCar.make"');
    const modelIndex = fieldsSection.indexOf('v-model="editCar.model"');
    const colorIndex = fieldsSection.indexOf('v-model="editCar.color"');

    expect(yearIndex).toBeGreaterThan(-1);
    expect(makeIndex).toBeGreaterThan(-1);
    expect(modelIndex).toBeGreaterThan(-1);
    expect(colorIndex).toBeGreaterThan(-1);

    expect(yearIndex).toBeLessThan(makeIndex);
    expect(makeIndex).toBeLessThan(modelIndex);
    expect(modelIndex).toBeLessThan(colorIndex);
  });
});
