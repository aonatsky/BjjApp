export default class DateHelper {
  static getCurrentDate(): Date {
    const today = new Date();
    return this.getDate(today);
  }

  static getDate(date : Date):Date{
    const dateWithoutTime = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    return dateWithoutTime;
  }

  static getDefaultDateOfBirth() {
    const date = new Date();
    date.setFullYear(date.getFullYear() - 20);
    return date;
  }
}
