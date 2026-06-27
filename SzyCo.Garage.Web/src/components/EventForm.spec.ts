import eventFormSource from "./EventForm.vue?raw";

describe("EventForm.vue", () => {
  it("clears event fields after saving", () => {
    expect(eventFormSource).toContain("function resetForm()");
    expect(eventFormSource).toContain("CarId: null");
    expect(eventFormSource).toContain("EventTypeId: null");
    expect(eventFormSource).toContain('PartName: ""');
    expect(eventFormSource).toContain("resetForm();");
  });
});
