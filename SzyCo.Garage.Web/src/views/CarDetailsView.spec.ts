import carDetailsViewSource from "./CarDetailsView.vue?raw";

describe("CarDetailsView.vue", () => {
  it("shows a total event history cost above event history", () => {
    const totalCostIndex = carDetailsViewSource.indexOf(
      ':total-event-history-cost="totalEventHistoryCost"',
    );
    const eventHistoryIndex = carDetailsViewSource.indexOf("Event History");

    expect(totalCostIndex).toBeGreaterThan(-1);
    expect(carDetailsViewSource).toContain("totalEventHistoryCost");
    expect(carDetailsViewSource).toContain("car.value.totalEventHistoryCost");
    expect(totalCostIndex).toBeLessThan(eventHistoryIndex);
  });

  it("keeps Edit Car field order aligned with Add Car", () => {
    const editDialogStart = carDetailsViewSource.indexOf(
      "<!-- Edit Car Dialog -->",
    );
    const editDialogTemplate = carDetailsViewSource.slice(editDialogStart);
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

  it("refreshes the car hero after saving edits", () => {
    expect(carDetailsViewSource).toContain("<CarHero");
    expect(carDetailsViewSource).toContain(':car-id="carId"');
    expect(carDetailsViewSource).toContain(':refresh-key="carRefreshKey"');
    expect(carDetailsViewSource).toContain("carRefreshKey.value += 1");
  });

  it("exposes edit, copy, and delete actions for events", () => {
    expect(carDetailsViewSource).toContain(':event="event"');
    expect(carDetailsViewSource).toContain("event-action-menu");
    expect(carDetailsViewSource).toContain('icon="fa fa-ellipsis-v"');
    expect(carDetailsViewSource).toContain('prepend-icon="fa fa-pencil"');
    expect(carDetailsViewSource).toContain('prepend-icon="fa fa-copy"');
    expect(carDetailsViewSource).toContain('prepend-icon="fa fa-trash"');
    expect(carDetailsViewSource).toContain('title="Edit event"');
    expect(carDetailsViewSource).toContain("Copy to today");
    expect(carDetailsViewSource).toContain('title="Delete event"');
    expect(carDetailsViewSource).toContain("confirmRemoveEvent");
  });

  it("copies an event as a new event for today", () => {
    expect(carDetailsViewSource).toContain("async function copyEvent");
    expect(carDetailsViewSource).toContain("EventServiceViewModel");
    expect(carDetailsViewSource).toContain("const eventService = new EventServiceViewModel()");
    expect(carDetailsViewSource).toContain(
      "await eventService.copyEventToToday(event.id)",
    );
    expect(carDetailsViewSource).toContain("Event copied to today.");
  });
});
